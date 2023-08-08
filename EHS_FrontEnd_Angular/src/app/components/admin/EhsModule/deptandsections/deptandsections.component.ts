import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MasterService } from 'src/app/services/masters/master.service';

@Component({
  selector: 'app-deptandsections',
  templateUrl: './deptandsections.component.html',
  styleUrls: ['./deptandsections.component.scss']
})
export class DeptandsectionsComponent {

  DepartmentList: any = []
  displayModal: boolean = false;
  addModal: boolean = false;
  editDepartmentName: string = '';
  departmentId: Number;
  departmentName: string = '';

  constructor(private router: Router, private masterservice: MasterService) { }


  ngOnInit(): void {
    this.getAllDepartment();
  }

  addDepartment() {
    let data = {
      DepartmentName: this.departmentName
    }
    setTimeout(() => {
      this.masterservice.masterPostMethod("/department/create", data).subscribe((res) => {
        this.getAllDepartment();
      })
    }, 1000);
  }


  getAllDepartment() {
    this.masterservice.masterGetMethod("/department/getall").subscribe((res) => {
      this.DepartmentList = res.result;
    });
  }




  EditId: number;
  editName: string;
  showModalDialog(row: any) {
    this.displayModal = true;
    const id = row.departmentId;
    this.EditId = id;
    this.editName = row.departmentName;
  }

  showAddDialog() {
    this.addModal = true;
  }

  editDepartment() {
    let data = {
      Id: this.EditId,
      name: this.editDepartmentName
    }
    setTimeout(() => {
      this.masterservice.masterPostMethod("/department/update", data).subscribe((res) => {
        this.getAllDepartment();
      })
    }, 2000);
  }

  deleteRow(row: any) {
    const id = row.departmentId;
    this.departmentId = id
    setTimeout(() => {
      this.masterservice.masterPostMethod(`/department/delete?id=${this.departmentId}`, null).subscribe((res) => {
        this.getAllDepartment();
      })
    }, 1000);
  }

}
