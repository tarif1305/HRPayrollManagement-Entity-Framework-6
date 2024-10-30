using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRPayrollManagement.Models;

namespace HRPayrollManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeDatabase db = new EmployeeDatabase();

        // GET: Employee
        public async Task<ActionResult> Index()
        {
            return View(await db.Employees.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeId,Name,Email,DateOfBirth,IsActive,ImagePath,Operation,Payrolls,Upload")] Employee employee)
        {

            if (employee.Operation == "Add")
            {
                employee.Payrolls.Add(new Payroll());
                return View(employee);
            }
            else if (employee.Operation.StartsWith("Delete"))
            {
                int.TryParse(employee.Operation.Replace("Delete-", ""), out int index);
                employee.Payrolls.RemoveAt(index);
                return View(employee);

            }
            if (employee.Upload != null)
            {
                string localPath = "/content/images/" + employee.Upload.FileName;
                string filePath = Server.MapPath(localPath);
                employee.Upload.SaveAs(filePath);
                employee.ImagePath = localPath;
                TempData["ImagePath"] = localPath;
            }
            else
            {
                if (TempData["ImagePath"] != null)
                {
                    employee.ImagePath = TempData["ImagePath"].ToString();
                }
            }
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeId,Name,Email,DateOfBirth,IsActive,ImagePath,Operation,Payrolls,Upload")] Employee employee)
        {
            if (employee.Operation == "Add")
            {
                employee.Payrolls.Add(new Payroll());
                return View(employee);
            }
            else if (employee.Operation.StartsWith("Delete"))
            {
                int.TryParse(employee.Operation.Replace("Delete-", ""), out int index);
                employee.Payrolls.RemoveAt(index);
                return View(employee);
            }


            if (employee.Upload != null)
            {
                string localPath = "/content/images/" + employee.Upload.FileName;
                string filePath = Server.MapPath(localPath);
                employee.Upload.SaveAs(filePath);
                employee.ImagePath = localPath;

            }
            if (ModelState.IsValid)
            {
                var del = await db.Employees.Include(a => a.Payrolls).FirstAsync(d => d.EmployeeId == employee.EmployeeId);



                db.Payrolls.RemoveRange(del.Payrolls);
                db.Employees.Remove(del);
                await db.SaveChangesAsync();

                db.Employees.Add(employee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            db.Payrolls.RemoveRange(employee.Payrolls);
            db.Employees.Remove(employee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
