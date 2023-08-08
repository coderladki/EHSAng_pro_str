import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LazyLoadEvent } from 'primeng/api';
import { MasterInterFace } from 'src/app/masterInterface';
import { MasterService } from 'src/app/services/masters/master.service';
@Component({
  selector: 'app-employee-master',
  templateUrl: './employee-master.component.html',
  styleUrls: ['./employee-master.component.scss']
})
export class EmployeeMasterComponent {
  loading: boolean = false;
  Unit: any = [];
  selectedUnit: any = []
  getEmployees: MasterInterFace[] = [];
  dob: Date;
  fName: string = '';
  lName: string = '';
  departmentName: string = '';
  designationName: string = '';
  unitName: string = '';
  mobileNumber: string = '';
  emailId: string = '';
  otherEmail: string = '';
  password: string = '';
  agencyNumber: string = '';
  gender: number;
  xEmployee: boolean;
  displayModal: boolean = false;
  displayEditModal: boolean = false;

  editDOB: Date;
  editFName: string = '';
  editLName: string = '';
  editDepartmentName: string = '';
  editDesignationName: string = '';
  editUnitName: string = '';
  editMobileNumber: string = '';
  editEmailId: string = '';
  editOtherEmail: string = '';
  editPassword: string = '';
  editAgencyNumber: string = '';
  editXEmployee: boolean;
  editGender: number;

  constructor(private router: Router, private masterservice: MasterService) { }

  ngOnInit(): void {
    this.getAllEmployee();
    this.getAllUnit();
  }

  getAllEmployee() {
    this.masterservice.masterGetMethod("/employee/getall").subscribe((res) => {
      this.getEmployees = res.Result;
    });
  }

  getAllUnit() {
    this.masterservice.masterGetMethod("/unit/getall").subscribe((res) => {
      this.Unit = res.result;
    });
  }

  addEmployee() {
    this.loading = true;
    let data = {
      DateofBirth: this.dob,
      FirstName: this.fName,
      LastName: this.lName,
      Department: this.departmentName,
      Designation: this.designationName,
      Unit: this.selectedUnit,
      Mobile: this.mobileNumber,
      Email: this.emailId,
      Password: this.password,
      Other: this.otherEmail,
      Agency: this.agencyNumber,
      XEmployee: this.xEmployee,
      Gender: this.gender
    }
    setTimeout(() => {
      this.masterservice.masterPostMethod("/employee/create", data).subscribe((res) => {
        this.getAllEmployee();
        this.loading = false;
      })
    }, 1000);
  }

  employeeId: Number;
  deleteRow(row: any) {
    this.loading = true;
    const id = row.EmployeeId;
    this.employeeId = id
    setTimeout(() => {
      this.masterservice.masterPostMethod(`/employee/delete?id=${this.employeeId}`, null).subscribe((res) => {
        this.getAllEmployee();
        this.loading = false;
      })
    }, 1000);
  }

  showModalDialog() {
    this.displayModal = true;
  }

  EditId: number;
  editName: string;
  showEditDialog(row: any) {
    this.displayEditModal = true;
    this.loading = true;
    const id = row.EmployeeId;
    this.EditId = id;
    this.editDOB = new Date(row.DateofBirth);
    this.editFName = row.FirstName;
    this.editLName = row.LastName;
    this.editDepartmentName = row.Department;
    this.editDesignationName = row.Designation;
    this.editUnitName = row.Unit;
    this.editMobileNumber = row.Mobile;
    this.editEmailId = row.Email;
    this.editPassword = row.Password;
    this.editOtherEmail = row.Other;
    this.editAgencyNumber = row.Agency;
    this.editXEmployee = row.XEmployee;
    this.editGender = row.Gender;
  }

  editEmployee() {
    this.loading = true;
    let data = {
      EmployeeId: this.EditId,
      DateofBirth: this.editDOB,
      FirstName: this.editFName,
      LastName: this.editLName,
      Department: this.editDepartmentName,
      Designation: this.editDesignationName,
      Unit: this.selectedUnit,
      Mobile: this.editMobileNumber,
      Email: this.editEmailId,
      Password: this.editPassword,
      Other: this.editOtherEmail,
      Agency: this.editAgencyNumber,
      XEmployee: this.editXEmployee,
      Gender: this.editGender
    }
    setTimeout(() => {
      this.masterservice.masterPostMethod("/employee/update", data).subscribe((res) => {
        this.getAllEmployee();
        this.loading = false;
      })
    }, 2000);
  }

  loadCustomers(event: LazyLoadEvent) {
    this.deleteRow('');
  }

}