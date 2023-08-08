import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, throwError } from 'rxjs';
import { UtilityService } from 'src/app/utility/utility.service';
import { MasterService } from '../../../../services/masters/master.service';

@Component({
    selector: 'app-add-role',
    templateUrl: './add-role.component.html',
    styleUrls: ['./add-role.component.scss'],
})
export class AddRoleComponent implements OnInit, OnDestroy {
    RoleName: string = '';
    RoleId: string = '';
    public isShowSubmitBtn: boolean = false;
    public queryParams: any = null;
    private unsubscribe: Subscription[] = [];
    private NavigateUrl: string = '/roles/role-list';
    constructor(
        private masterService: MasterService,
        private router: Router,
        private route: ActivatedRoute,
        private utility: UtilityService
    ) {}
    ngOnInit(): void {
        const queryParmsSub = this.route.queryParams.subscribe((params) => {
            this.queryParams = params;
        });

        this.unsubscribe.push(queryParmsSub);
        if (this.ifExistRoleId()) {
            this.isShowSubmitBtn = false;
            this.getRoleById();
        } else {
            this.isShowSubmitBtn = true;
        }
    }
    ifExistRoleId(): boolean {
        return JSON.stringify(this.queryParams) != '{}';
    }
    onSubmit() {
        if (this.RoleName == null || this.RoleName == '') {
            return;
        }
        var body = {
            RoleName: this.RoleName,
        };
        const addRole = this.masterService
            .masterPostMethod('/roles/create', body)
            .subscribe(
                (res) => {
                    if (res.succeeded == true) {
                        alert('Role Created Successfully.');
                        this.router.navigate([this.NavigateUrl], {
                            relativeTo: this.route,
                        });
                    } else {
                        alert(res[0].description);
                    }
                },
                (errors) => {
                    let errorMsg = '';
                    var currentIndex;
                    if (
                        errors.errors != undefined &&
                        errors.errors != null &&
                        errors.errors.length > 0
                    ) {
                        errors.errors.forEach((e: string) => {
                            errorMsg += e + ' ,';
                        });
                        if (
                            errorMsg.indexOf(
                                ',',
                                errorMsg.length - ','.length
                            ) !== -1
                        ) {
                            errorMsg = errorMsg.substring(
                                0,
                                errorMsg.length - 1
                            );
                        }

                        alert(errorMsg);
                    }
                }
            );
        this.unsubscribe.push(addRole);
    }
    getRoleById() {
        const getrolebyidsub = this.masterService
            .masterGetMethod('/roles/' + this.queryParams.roleId)
            .subscribe((res) => {
                this.RoleId = res.id;
                this.RoleName = res.roleName;
            });
        this.unsubscribe.push(getrolebyidsub);
    }

    onCancel() {
        this.router.navigateByUrl(this.NavigateUrl);
    }
    onUpdate() {
        var body = {
            Id: this.RoleId,
            RoleName: this.RoleName,
        };
        this.utility.showSpinner();
        const updateRoleByIdSub = this.masterService
            .masterPostMethod('/roles/edit', body)
            .subscribe((res) => {
                this.utility.hideSpinner();
                alert('Updated Successfully');
                this.router.navigateByUrl(this.NavigateUrl);
            });
        this.unsubscribe.push(updateRoleByIdSub);
    }
    onDelete() {
        throw new Error('Method not implemented');
    }
    ngOnDestroy() {
        this.unsubscribe.forEach((sb) => sb.unsubscribe());
    }
}
