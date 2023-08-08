import {
    AfterViewInit,
    ChangeDetectorRef,
    Component,
    OnDestroy,
    OnInit,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, throwError } from 'rxjs';
import { PermissionService } from '../../../../services/Permission/permission.service';
import { UtilityService } from 'src/app/utility/utility.service';
import { MasterService } from '../../../../services/masters/master.service';
import { Permission } from 'src/app/interfaces/Permission.class';

@Component({
    selector: 'app-role-permission',
    templateUrl: './role-permission.component.html',
    styleUrls: ['./role-permission.component.scss'],
})
export class RolePermissionComponent
    implements OnInit, OnDestroy, AfterViewInit
{
    roleDetails: any = {};
    public queryParams: any = null;
    private roleId: number = 0;
    public NavigateUrl: string = '/roles/role-list';

    AssignedPermissions: string[] = [];
    PermissionList: Permission[] = [];

    AssignedNavigationPermissions: string[] = [];
    NavigationList: Permission[] = [];
    showList: boolean = false;
    permissionCheckList: { Name: string; id: number; Selected: boolean }[] = [];
    toFilterpermissionlist: any;
    navigationPermissionCheckList: {
        Name: string;
        id: number;
        Selected: boolean;
    }[] = [];

    masterSelectedPermission: boolean = false;
    isAnyPermissionSelected: boolean = false;
    isAllPermissionSelected: boolean = false;

    masterSelectedNavigationPermission: boolean = false;
    isAnyNavigationPermissionSelected: boolean = false;
    isAllNavigationPermissionSelected: boolean = false;
    constructor(
        private masterService: MasterService,
        private router: Router,
        private route: ActivatedRoute,
        private utility: UtilityService,
        private permissionService: PermissionService,
        private cdRef: ChangeDetectorRef
    ) {}
    ngAfterViewInit(): void {
        this.getPermissionsAssignedToRole();
        this.getNavigationPermissionsAssignedToRole();
    }

    ngOnInit(): void {
        const queryParmsSub = this.route.queryParams.subscribe((params) => {
            this.queryParams = params;
        });
        this.getRoleDetails(this.queryParams.roleId);
    }
    getRoleDetails(id: any) {
        this.utility.showSpinner();
        this.masterService.masterGetMethod(`/roles/${id}`).subscribe((res) => {
            this.roleDetails = res;
            this.utility.hideSpinner();
            this.cdRef.detectChanges();
        });
    }

    onRoleUsers() {
        this.router.navigate(['roles/update-role-users'], {
            queryParams: {
                roleId: this.queryParams.roleId,
            },
        });
    }
    getAllPermissions() {
        this.utility.showSpinner();
        this.showList = false;
        this.permissionService.getAllPermissions.subscribe(
            (res) => {
                this.utility.hideSpinner();
                this.PermissionList = res;
                for (let p of this.PermissionList) {
                    if (
                        this.AssignedPermissions.find((ap) => ap == p.name) ==
                        undefined
                    ) {
                        this.permissionCheckList.push({
                            Name: p.name,
                            id: p.navigationid,
                            Selected: false,
                        });
                        this.toFilterpermissionlist = this.permissionCheckList;
                    } else {
                        this.permissionCheckList.push({
                            Name: p.name,
                            id: p.navigationid,
                            Selected: true,
                        });
                        this.toFilterpermissionlist = this.permissionCheckList;
                    }
                }
                this.isAllPremissionSelected();
                this.cdRef.detectChanges();
            },
            (error) => {
                this.PermissionList = [];
                this.cdRef.detectChanges();
            }
        );
    }
    getPermissionsAssignedToRole() {
        this.utility.showSpinner();
        this.permissionService
            .getAllPermissionsAssignedToRole(this.queryParams.roleId)
            .subscribe(
                (res) => {
                    this.utility.hideSpinner();
                    this.AssignedPermissions = res;
                    this.getAllPermissions();
                },
                (error) => {
                    this.AssignedPermissions = [];
                    this.cdRef.detectChanges();
                }
            );
    }
    isAllPremissionSelected() {
        this.isAnyPermissionSelected = this.permissionCheckList.filter(
            (a) => a.Selected == true
        ).length
            ? true
            : false;
        if (
            this.permissionCheckList.filter((a) => a.Selected == true).length >
                0 &&
            this.permissionCheckList.filter((a) => a.Selected == false)
                .length == 0
        ) {
            this.isAllPermissionSelected = true;
        } else {
            this.isAllPermissionSelected = false;
        }
    }
    getNavigationPermissionsAssignedToRole() {
        this.utility.showSpinner();
        this.permissionService
            .getAllNavigationPermissionsAssignedToRole(this.queryParams.roleId)
            .subscribe(
                (res) => {
                    this.utility.hideSpinner();
                    this.AssignedNavigationPermissions = res;
                    console.log(this.AssignedNavigationPermissions);
                    this.getAllNavigation();
                },
                (error) => {
                    this.AssignedNavigationPermissions = [];
                    this.cdRef.detectChanges();
                }
            );
    }
    getAllNavigation() {
        this.utility.showSpinner();
        this.permissionService.getAllNavigations.subscribe(
            (res) => {
                this.utility.hideSpinner();
                this.NavigationList = res;
                console.log(this.NavigationList);
                for (let p of this.NavigationList) {
                    if (
                        this.AssignedNavigationPermissions.find(
                            (ap) => ap == p.name
                        ) == undefined
                    ) {
                        this.navigationPermissionCheckList.push({
                            Name: p.name,
                            id: p.crmmenuid,
                            Selected: false,
                        });
                    } else {
                        this.navigationPermissionCheckList.push({
                            Name: p.name,
                            id: p.crmmenuid,
                            Selected: true,
                        });
                    }
                }
                this.isAllNavigationPremissionSelected();
                this.cdRef.detectChanges();
            },
            (error) => {
                this.PermissionList = [];
                this.cdRef.detectChanges();
            }
        );
    }
    isAllNavigationPremissionSelected() {
        this.isAnyNavigationPermissionSelected =
            this.navigationPermissionCheckList.filter((a) => a.Selected == true)
                .length
                ? true
                : false;
        if (
            this.navigationPermissionCheckList.filter((a) => a.Selected == true)
                .length > 0 &&
            this.navigationPermissionCheckList.filter(
                (a) => a.Selected == false
            ).length == 0
        ) {
            this.isAllNavigationPermissionSelected = true;
        } else {
            this.isAllNavigationPermissionSelected = false;
        }
    }
    allPremissionSelected() {
        this.masterSelectedPermission = this.permissionCheckList.every(
            function (item: any) {
                return item.isSelected == true;
            }
        );
        this.isAllPremissionSelected();
    }
    checkUncheckAllPermission() {
        for (var i = 0; i < this.permissionCheckList.length; i++) {
            this.permissionCheckList[i].Selected =
                this.masterSelectedPermission;
        }
        this.isAllPremissionSelected();
    }
    allNavigationPremissionSelected(id: number, status: boolean) {
        this.masterSelectedNavigationPermission =
            this.navigationPermissionCheckList.every(function (item: any) {
                return item.isSelected == true;
            });

        this.isAllNavigationPremissionSelected();
    }
    check(id: number) {
        this.showList = true;
        this.permissionCheckList = this.toFilterpermissionlist.filter(
            (x: any) => x.id == id
        );
    }
    checkUncheckAllNavigationPermission() {
        for (var i = 0; i < this.navigationPermissionCheckList.length; i++) {
            this.navigationPermissionCheckList[i].Selected =
                this.masterSelectedNavigationPermission;
        }
        this.isAllNavigationPremissionSelected();
    }
    UpdateNavigationPermissions() {
        let body: string[] = [];
        this.navigationPermissionCheckList
            .filter((a) => a.Selected == true)
            .forEach((item) => {
                body.push(item.Name);
            });

        this.utility.showSpinner();
        this.masterService
            .masterPostMethod(
                `/Permissions/AddNavigationPermissions/Role/${this.queryParams.roleId}`,
                body
            )
            .subscribe(
                (res) => {
                    if (res.status == 'ok') {
                        this.utility.hideSpinner();
                        //alert(res.message);
                        this.navigationPermissionCheckList = [];
                        this.getNavigationPermissionsAssignedToRole();
                    }
                },
                (error) => {
                    console.log(error);
                }
            );
    }
    UpdateRolePermissions() {
        let body: string[] = [];
        this.toFilterpermissionlist
            .filter((a: any) => a.Selected == true)
            .forEach((item: any) => {
                body.push(item.Name);
            });

        this.utility.showSpinner();
        this.masterService
            .masterPostMethod(
                `/Permissions/AddPermissions/Role/${this.queryParams.roleId}`,
                body
            )
            .subscribe(
                (res) => {
                    if (res.status == 'ok') {
                        this.utility.hideSpinner();
                        this.permissionCheckList = [];
                        this.getPermissionsAssignedToRole();
                    }
                },
                (error) => {
                    console.log(error);
                }
            );
    }
    updateRolesandPermission() {
        try {
            this.UpdateNavigationPermissions();
            this.UpdateRolePermissions();
            //alert("Updated Succesfully");
        } catch (error) {
            alert(error);
        }
    }
    ngOnDestroy(): void {}
}
