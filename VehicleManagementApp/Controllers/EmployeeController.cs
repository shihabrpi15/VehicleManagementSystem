﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        private IEmployeeManager _employeeManager;
        private IDepartmentManager _departmentManager;
        private IDesignationManager _designationManager;
        private IDivisionManager _divisionManager;
        private IDistrictManager _districtManager;
        private IThanaManager _thanaManager;

        public EmployeeController(IEmployeeManager employee, IDepartmentManager department, IDesignationManager designation,
            IDivisionManager division, IDistrictManager district, IThanaManager thana)
        {
            this._employeeManager = employee;
            this._departmentManager = department;
            this._designationManager = designation;
            this._divisionManager = division;
            this._districtManager = district;
            this._thanaManager = thana;
        }

        public ActionResult Index()
        {
            var department = _departmentManager.GetAll();
            var designation = _designationManager.GetAll();
            var employee = _employeeManager.GetAll();
            var division = _divisionManager.GetAll();
            var district = _districtManager.GetAll();
            var thana = _thanaManager.GetAll();

            List<EmployeeViewModel> employeeViewList = new List<EmployeeViewModel>();
            foreach (var emploeedata in employee)
            {
                var employeeVM = new EmployeeViewModel();
                employeeVM.Id = emploeedata.Id;
                employeeVM.Name = emploeedata.Name;
                employeeVM.ContactNo = emploeedata.ContactNo;
                employeeVM.Email = emploeedata.Email;
                employeeVM.Address1 = emploeedata.Address1;
                employeeVM.Address2 = employeeVM.Address2;
                employeeVM.LicenceNo = emploeedata.LicenceNo;
                employeeVM.Department = department.Where(x => x.Id == emploeedata.DepartmentId).FirstOrDefault();
                employeeVM.Designation = designation.Where(x => x.Id == emploeedata.DesignationId).FirstOrDefault();
                employeeVM.Division = division.Where(x => x.Id == emploeedata.DivisionId).FirstOrDefault();
                employeeVM.District = district.Where(x => x.Id == emploeedata.DistrictId).FirstOrDefault();
                employeeVM.Thana = thana.Where(x => x.Id == emploeedata.ThanaId).FirstOrDefault();

                employeeViewList.Add(employeeVM);
            }
            return View(employeeViewList);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            var department = _departmentManager.GetAll();
            var designation = _designationManager.GetAll();
            var division = _divisionManager.GetAll();
            var district = _districtManager.GetAll();
            var thana = _thanaManager.GetAll();

            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.Departments = department;
            employeeVM.Designations = designation;
            employeeVM.Divisions = division;
            employeeVM.Districts = district;
            employeeVM.Thanas = thana;


            ViewBag.districtDropDown = new SelectListItem[] {new SelectListItem() {Value="", Text="Select..."} };

            //ViewBag.districtDropDown = new SelectLsistItem[] { new SelectListItem() { Value = "", Text = "Select..." } };

            return View(employeeVM);
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeViewModel employeeVM)
        {
            try
            {
                Employee employee = new Employee();
                employee.Name = employeeVM.Name;
                employee.ContactNo = employeeVM.ContactNo;
                employee.Email = employeeVM.Email;
                employee.Address1 = employeeVM.Address1;
                employee.Address2 = employeeVM.Address2;
                employee.LicenceNo = employeeVM.LicenceNo;
                employee.DepartmentId = employeeVM.DepartmentId;
                employee.DesignationId = employeeVM.DesignationId;
                employee.DivisionId = employeeVM.DivisionId;
                employee.DistrictId = employeeVM.DivisionId;
                employee.ThanaId = employeeVM.ThanaId;

                bool isSaved = _employeeManager.Add(employee);
                if (isSaved)
                {
                    TempData["msg"] = "Employee Save Successfully";
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Employee employee = _employeeManager.GetById((int)id);

            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.Id = employee.Id;
            employeeVM.Name = employee.Name;
            employeeVM.ContactNo = employee.ContactNo;
            employeeVM.Email = employee.Email;
            employeeVM.Address1 = employee.Address1;
            employeeVM.Address2 = employeeVM.Address2;
            employeeVM.LicenceNo = employee.LicenceNo;
            employeeVM.DepartmentId = employee.DepartmentId;
            employeeVM.DesignationId = employee.DesignationId;
            employeeVM.DivisionId = employee.DivisionId;
            employeeVM.DistrictId = employee.DistrictId;
            employeeVM.ThanaId = employee.ThanaId;

            ViewBag.DepartmentId = new SelectList(_departmentManager.GetAll(),"Id","Name", employee.DepartmentId);
            ViewBag.DesignationId = new SelectList(_designationManager.GetAll(),"Id","Name", employee.DesignationId);
            ViewBag.DivisionId = new SelectList(_divisionManager.GetAll(), "Id", "Name", employee.DivisionId);
            ViewBag.DistrictId = new SelectList(_districtManager.GetAll(), "Id", "Name", employee.DistrictId);
            ViewBag.ThanaId = new SelectList(_thanaManager.GetAll(), "Id", "Name", employee.ThanaId);

            return View(employeeVM);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(EmployeeViewModel employeeVM)
        {
            try
            {
                Employee employee = new Employee();
                employee.Id = employeeVM.Id;
                employee.Name = employeeVM.Name;
                employee.ContactNo = employeeVM.ContactNo;
                employee.Email = employeeVM.Email;
                employee.Address1 = employeeVM.Address1;
                employee.Address2 = employeeVM.Address2;
                employee.LicenceNo = employeeVM.LicenceNo;
                employee.DepartmentId = employeeVM.DepartmentId;
                employee.DesignationId = employeeVM.DesignationId;
                employee.DivisionId = employeeVM.DivisionId;
                employee.DistrictId = employeeVM.DistrictId;
                employee.ThanaId = employeeVM.ThanaId;

                _employeeManager.Update(employee);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Employee employee = _employeeManager.GetById((int)id);
            _employeeManager.Remove(employee);
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
