import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorComponent } from '../../auth/error/error.component';
import { AddRoleComponent } from './add-role/add-role.component';
import { RoleListComponent } from './role-list/role-list.component';
import { RolePermissionComponent } from './role-permission/role-permission.component';
import { RolesComponent } from './roles.component';
import { UpdateRoleUsersComponent } from './update-role-users/update-role-users.component';

const routes: Routes = [
    {
        path: '',
        component: RolesComponent,
        children: [
            {
                path: 'add-role',
                component: AddRoleComponent,
            },
            {
                path: 'role-list',
                component: RoleListComponent,
            },
            {
                path: 'role-permission',
                component: RolePermissionComponent,
            },
            {
                path: 'update-role-users',
                component: UpdateRoleUsersComponent,
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RolesRoutingModule {}
