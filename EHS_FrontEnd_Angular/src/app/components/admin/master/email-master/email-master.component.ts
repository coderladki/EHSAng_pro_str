import { Component, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { LazyLoadEvent } from 'primeng/api';
import { MasterInterFace } from 'src/app/masterInterface';
import { MasterService } from 'src/app/services/masters/master.service';

@Component({
    selector: 'app-email-master',
    templateUrl: './email-master.component.html',
    styleUrls: ['./email-master.component.scss'],
})
export class EmailMasterComponent {
    getDivisions: MasterInterFace[] = [];
    selectedDivision: any;
    selectedType: any;
    getAllEmails: any[] = [];
    toEmails: string = '';
    ccEmails: string = '';
    displayModal: boolean = false;
    addModal: boolean = false;

    editSelectedDivision: number;
    editSelectedType: any;
    editToEmails: string;
    editCCEmails: string;

    constructor(private router: Router, private masterservice: MasterService) {}
    ngOnInit(): void {
        this.getDivision();
        this.getEmailList();
    }

    ngOnChanges(Chages: SimpleChanges) {
        this.getDivision();
        this.getEmailList();
    }

    getDivision() {
        this.masterservice
            .masterGetMethod('/division/getall')
            .subscribe((res) => {
                this.getDivisions = res.Result;
            });
    }

    getEmailList() {
        this.masterservice.masterGetMethod('/email/getall').subscribe((res) => {
            this.getAllEmails = res.result;
        });
    }

    addEmail() {
        this.loading = true;
        let data = {
            TypeId: 1,
            DivisionId: this.selectedDivision,
            To_Emails: this.toEmails,
            CC_Emails: this.ccEmails,
            Email_Status: 1,
        };
        setTimeout(() => {
            this.masterservice
                .masterPostMethod('/email/create', data)
                .subscribe((res) => {
                    this.getEmailList();
                    this.loading = false;
                });
        }, 1000);
    }

    emailMasterId: Number;
    deleteRow(row: any) {
        this.loading = true;
        const id = row.emailMasterId;
        this.emailMasterId = id;
        setTimeout(() => {
            this.masterservice
                .masterPostMethod(
                    `/email/delete?id=${this.emailMasterId}`,
                    null
                )
                .subscribe((res) => {
                    this.getEmailList();
                    this.loading = false;
                });
        }, 1000);
    }

    EditId: number;
    TypeId: number;
    Email_Status: number;
    showModalDialog(row: any) {
        this.displayModal = true;
        const id = row.emailMasterId;
        this.EditId = id;
        this.TypeId = 1;
        this.editSelectedDivision = row.divisionId;
        this.editToEmails = row.to_Emails;
        this.editCCEmails = row.cC_Emails;
    }

    editEmail() {
        this.loading = true;
        let data = {
            emailMasterId: this.EditId,
            TypeId: 1,
            DivisionId: this.editSelectedDivision,
            To_Emails: this.editToEmails,
            CC_Emails: this.editCCEmails,
            Email_Status: 1,
        };
        setTimeout(() => {
            this.masterservice
                .masterPostMethod('/email/update', data)
                .subscribe((res) => {
                    this.getEmailList();
                    this.loading = false;
                });
        }, 1000);
    }

    loading: boolean = false;
    loadCustomers(event: LazyLoadEvent) {
        this.deleteRow('');
        this.addEmail();
        this.editEmail();
    }

    showAddDialog() {
        this.addModal = true;
    }
}
