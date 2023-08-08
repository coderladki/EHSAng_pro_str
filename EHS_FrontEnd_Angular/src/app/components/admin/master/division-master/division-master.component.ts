import { Component, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { LazyLoadEvent } from 'primeng/api';
import { MasterInterFace } from 'src/app/masterInterface';
import { MasterService } from 'src/app/services/masters/master.service';

@Component({
  selector: 'app-division-master',
  templateUrl: './division-master.component.html',
  styleUrls: ['./division-master.component.scss']
})
export class DivisionMasterComponent {

  getDivisions: MasterInterFace[] = [];
  divisionName: string = '';
  editDivisionName: string = '';
  getDivisionName: any = '';
  Id: Number;
  displayModal: boolean = false;
  addModal: boolean = false;

  constructor(private router: Router, private masterservice: MasterService) { }
  ngOnInit(): void {
    this.getDivision();
  }

  ngOnChanges(Chages: SimpleChanges) {
    this.getDivision();
  }


  getDivision() {
    this.masterservice.masterGetMethod("/division/getall").subscribe((res) => {
      this.getDivisions = res.Result;
    });
  }

  addDivision() {
    this.loading = true;
    let data = {
      name: this.divisionName
    }
    setTimeout(() => {
      this.masterservice.masterPostMethod("/division/create", data).subscribe((res) => {
        this.getDivision();
        this.loading = false;
      })
    }, 1000);
  }

  deleteRow(row: any) {
    this.loading = true;
    const id = row.Id;
    this.Id = id
    setTimeout(() => {
      this.masterservice.masterPostMethod(`/division/delete?id=${this.Id}`, null).subscribe((res) => {
        this.getDivision();
        this.loading = false;
      })
    }, 1000);
  }

  EditId: number;
  editName: string;
  showModalDialog(row: any) {
    this.displayModal = true;
    this.loading = true;
    const id = row.Id;
    this.EditId = id;
    this.editName = row.Name;
  }

  showAddDialog() {
    this.addModal = true;
  }

  editDivision() {
    this.loading = true;
    let data = {
      Id: this.EditId,
      name: this.editDivisionName
    }
    setTimeout(() => {
      this.masterservice.masterPostMethod("/division/update", data).subscribe((res) => {
        this.getDivision();
        this.loading = false;
      })
    }, 2000);
  }

  loading: boolean = false;
  loadCustomers(event: LazyLoadEvent) {
    this.addDivision();
    this.deleteRow('');
  }

}