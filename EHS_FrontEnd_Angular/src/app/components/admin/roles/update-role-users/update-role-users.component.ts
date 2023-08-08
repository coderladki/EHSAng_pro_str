import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MasterService } from '../../../../services/masters/master.service';
import { UpdateRoleUsersModel } from '../../../../models/UpdateRoleUsersModel';
import { UtilityService } from 'src/app/utility/utility.service';

@Component({
    selector: 'app-update-role-users',
    templateUrl: './update-role-users.component.html',
    styleUrls: ['./update-role-users.component.scss'],
})
export class UpdateRoleUsersComponent implements OnInit {
    roleId: number;
    _queryParams: any;
    public model: UpdateRoleUsersModel;
    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private masterService: MasterService,
        private cdRef: ChangeDetectorRef,
        private utility: UtilityService
    ) {}
    ngOnInit(): void {
        this.route.queryParams.subscribe((params) => {
            this._queryParams = params;
        });
        this.roleId = this._queryParams.roleId;
        this.getDetails(this.roleId);
        //throw new Error('Method not implemented.');
    }
    getDetails(id: number) {
        this.utility.showSpinner();
        this.masterService
            .masterGetMethod(`/roles/getUsersInRole/${id}`)
            .subscribe((res) => {
                this.utility.hideSpinner();
                this.model = {
                    roleId: id,
                    userRoleModelList: res,
                };
                this.cdRef.detectChanges();
            });
    }
    updateUsersInRole() {
        this.utility.showSpinner();
        this.masterService
            .masterPostMethod(`/roles/editUsersInRole`, this.model)
            .subscribe((res) => {
                this.utility.hideSpinner();
                alert('Users in Role Updated Successfully.');
            });
    }
    onCancel() {
        this.router.navigate(['/roles/role-permission'], {
            queryParams: { roleId: this.roleId },
        });
    }
}
