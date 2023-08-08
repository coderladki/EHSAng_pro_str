import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LazyLoadEvent } from 'primeng/api';
import { UtilityService } from 'src/app/utility/utility.service';
import { MasterService } from '../../../../services/masters/master.service';

@Component({
    selector: 'app-user-list',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
    SearchName: string;
    UserList: any[];
    UserListFilter: any[];
    constructor(
        private fb: FormBuilder,
        public route: ActivatedRoute,
        private masterService: MasterService,
        private cd: ChangeDetectorRef,
        private router: Router,
        private utility: UtilityService
    ) { }

    ngOnInit(): void {
        this.getAllUsers();
    }

    public getAllUsers() {
        this.utility.showSpinner();
        let apiUrl = `/userss/getAll`;
        this.masterService.masterGetMethod(apiUrl).subscribe(
            (res) => {
                this.UserList = res;

                this.UserList.forEach((x: { status: string | number }) => {
                    if (x.status == '1') {
                        x.status = 'Active';
                    } else {
                        x.status = 'Inactive';
                    }
                });
                this.UserListFilter = this.UserList;
                this.utility.hideSpinner();
                this.cd.detectChanges();
            },
            (error) => {
                console.log(error);
            }
        );
    }
    applyFilter(e: any) {
        const filterValue = (e.target as HTMLInputElement).value;
        this.UserList = this.UserListFilter.filter(
            (x) =>
                x.firstName
                    .toUpperCase()
                    .indexOf(filterValue.trim().toUpperCase()) > -1 ||
                x.lastName
                    .toUpperCase()
                    .indexOf(filterValue.trim().toUpperCase()) > -1
        );
        this.cd.detectChanges();
    }

    loading: boolean = false;
    loadCustomers(event: LazyLoadEvent) {
        this.getAllUsers();
    }
}
