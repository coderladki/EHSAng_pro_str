import {
    AfterViewInit,
    ChangeDetectorRef,
    Component,
    OnInit,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
// import { Permission } from 'src/app/interfaces/Permission.class';
// import { MasterService } from 'src/app/services/master.service';
// import { PermissionService } from 'src/app/services/permission.service';
// import { RoleService } from 'src/app/services/role.service';
// import { SnackBarComponent } from 'src/shared/snack-bar-message/snack-bar-messagaes.component';
import { Permission } from '../../../../interfaces/Permission.class';
import { MasterService } from '../../../../services/masters/master.service';
import { PermissionService } from '../../../../services/Permission/permission.service';
import { RoleService } from '../../../../services/Role/role.service';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';

@Component({
    selector: 'app-edit-user',
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.scss'],
})
export class EditUserComponent implements OnInit, AfterViewInit {
    spinner: boolean;
    id: number;
    public userDetails: any;

    AssignedPermissions: string[] = [];
    PermissionList: Permission[] = [];

    SelectedRolesList: any;
    RoleSetting = {
        singleSelection: false,
        idField: 'id',
        textField: 'name',
        selectAllText: 'Select All',
        unSelectAllText: 'UnSelect All',
        itemsShowLimit: 3,
        allowSearchFilter: true,
    };
    RoleList: any[];

    AssignedNavigationPermissions: string[] = [];
    NavigationList: Permission[] = [];

    permissionCheckList: { Name: string; id: number; Selected: boolean }[] = [];
    //navigationPermissionCheckList: { Name: string, Selected: boolean }[] = [];
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

    isLoading: boolean = false;
    constructor(
        private route: ActivatedRoute,
        private masterService: MasterService,
        private permissionService: PermissionService,
        private cdRef: ChangeDetectorRef,
        private roleService: RoleService //private snackBar: SnackBarComponent
    ) {}

    ngOnInit(): void {
        this.route.paramMap.subscribe((params) => {
            this.id = Number(params.get('userId'));
        });

        //this.id = this.route.snapshot.params.userId;
        this.getUserDetails(this.id);
    }
    ngAfterViewInit(): void {
        this.roleService.RoleList.subscribe(
            (res) => {
                this.spinner = false;
                this.RoleList = res;
                console.log(this.RoleList);
                this.cdRef.detectChanges();
            },
            (error) => {
                this.spinner = false;
                this.RoleList = [];
                this.cdRef.detectChanges();
            }
        );
        this.getPermissionsAssignedToUser();
        this.getNavigationPermissionsAssignedToUser();
    }

    UpdateUserPermissions() {
        let body: string[] = [];
        // this.permissionCheckList.filter(a => a.Selected == true).forEach(item => {
        //   body.push(item.Name)
        // });
        this.toFilterpermissionlist
            .filter((a: any) => a.Selected == true)
            .forEach((item: any) => {
                body.push(item.Name);
            });

        this.spinner = true;
        this.masterService
            .masterPostMethod(
                `/Permissions/AddPermissions/User/${this.id}`,
                body
            )
            .subscribe(
                (res) => {
                    if (res.status == 'ok') {
                        this.spinner = false;
                        alert(res.message);
                        this.AssignedPermissions = [];
                        this.PermissionList = [];
                        this.getPermissionsAssignedToUser();
                    }
                },
                (error) => {
                    console.log(error);
                }
            );
    }
    getAllPermissions() {
        this.spinner = true;
        this.showList = false;
        this.permissionService.getAllPermissions.subscribe(
            (res) => {
                this.spinner = false;
                this.PermissionList = res;
                for (let p of this.PermissionList) {
                    if (
                        this.AssignedPermissions.find((ap) => ap == p.name) ==
                        undefined
                    ) {
                        // this.permissionCheckList.push({ Name: p.name, Selected: false });
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
                        //this.permissionCheckList.push({ Name: p.name, Selected: true });
                    }
                }
                this.cdRef.detectChanges();
            },
            (error) => {
                this.PermissionList = [];
                this.cdRef.detectChanges();
            }
        );
    }
    getPermissionsAssignedToUser() {
        this.spinner = true;
        this.permissionService
            .getAllPermissionsAssignedToUser(this.id)
            .subscribe(
                (res) => {
                    this.spinner = false;
                    this.AssignedPermissions = res;
                    this.getAllPermissions();
                },
                (error) => {
                    this.AssignedPermissions = [];
                    this.cdRef.detectChanges();
                }
            );
    }
    getUserDetails(id: any) {
        this.masterService
            .masterGetMethod(`/users/get/${id}`)
            .subscribe((res) => {
                this.userDetails = res;
                this.cdRef.detectChanges();
            });
    }
    UpdateUserRoles() {}
    RoleChange() {
        //this.SelectedRole = Role
        // console.log("Role Change", this.SelectedRole)
        this.cdRef.detectChanges();
    }
    showList: boolean = false;
    toFilterpermissionlist: any;
    check(id: number) {
        this.showList = true;
        this.permissionCheckList = this.toFilterpermissionlist.filter(
            (x: any) => x.id == id
        );
    }
    EditUser() {
        // var body = {
        //   Id: this.id,
        //   RoleName: this.roleDetails.roleName
        // }
        // this.spinner = true
        // this.masterService.masterPostMethod('/roles/edit', body).subscribe(
        //   res => {
        //     this.spinner = false
        //     alert("Updated Successfully")
        //   }
        // )
    }
    onSelectAllRoles(e: any) {}

    //permission
    checkUncheckAllPermission() {
        for (var i = 0; i < this.permissionCheckList.length; i++) {
            this.permissionCheckList[i].Selected =
                this.masterSelectedPermission;
        }
        this.isAllPremissionSelected();
    }

    allPremissionSelected() {
        this.masterSelectedPermission = this.permissionCheckList.every(
            function (item: any) {
                return item.isSelected == true;
            }
        );
        this.isAllPremissionSelected();
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

    //navigation permission
    checkUncheckAllNavigationPermission() {
        for (var i = 0; i < this.navigationPermissionCheckList.length; i++) {
            this.navigationPermissionCheckList[i].Selected =
                this.masterSelectedNavigationPermission;
        }
        this.isAllNavigationPremissionSelected();
    }

    allNavigationPremissionSelected(id: number, status: boolean) {
        this.masterSelectedNavigationPermission =
            this.navigationPermissionCheckList.every(function (item: any) {
                return item.isSelected == true;
            });
        this.isAllNavigationPremissionSelected();
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

    getNavigationPermissionsAssignedToUser() {
        this.spinner = true;
        this.permissionService
            .getAllNavigationPermissionsAssignedToUser(this.id)
            .subscribe(
                (res) => {
                    this.spinner = false;
                    this.AssignedNavigationPermissions = res;
                    this.getAllNavigation();
                },
                (error) => {
                    this.AssignedNavigationPermissions = [];
                    this.cdRef.detectChanges();
                }
            );
    }

    getAllNavigation() {
        this.spinner = true;
        this.permissionService.getAllNavigations.subscribe(
            (res) => {
                this.spinner = false;
                this.NavigationList = res;
                for (let p of this.NavigationList) {
                    if (
                        this.AssignedNavigationPermissions.find(
                            (ap) => ap == p.name
                        ) == undefined
                    ) {
                        // this.navigationPermissionCheckList.push({ Name: p.name, Selected: false });
                        this.navigationPermissionCheckList.push({
                            Name: p.name,
                            id: p.crmmenuid,
                            Selected: false,
                        });
                    } else {
                        //this.navigationPermissionCheckList.push({ Name: p.name, Selected: true });
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

    UpdateUserNavigationPermissions() {
        let body: string[] = [];
        this.navigationPermissionCheckList
            .filter((a) => a.Selected == true)
            .forEach((item) => {
                body.push(item.Name);
            });

        this.spinner = true;
        this.masterService
            .masterPostMethod(
                `/Permissions/AddNavigationPermissions/User/${this.id}`,
                body
            )
            .subscribe(
                (res) => {
                    if (res.status == 'ok') {
                        this.spinner = false;
                        alert(res.message);
                        this.navigationPermissionCheckList = [];
                        this.getNavigationPermissionsAssignedToUser();
                    }
                },
                (error) => {
                    console.log(error);
                }
            );
    }
}
