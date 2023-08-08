import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RolesRoutingModule } from './roles-routing.module';
import { AddRoleComponent } from './add-role/add-role.component';
import { RoleListComponent } from './role-list/role-list.component';
import { RolesComponent } from './roles.component';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PanelModule } from 'primeng/panel';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { UtilityModule } from 'src/app/utility/utility.module';
import { SpinnerComponent } from 'src/app/utility/spinner/spinner.component';
import { RolePermissionComponent } from './role-permission/role-permission.component';
import { UpdateRoleUsersComponent } from './update-role-users/update-role-users.component';
import { MessageService, PrimeNGConfig } from 'primeng/api';

@NgModule({
    declarations: [
        AddRoleComponent,
        RoleListComponent,
        RolesComponent,
        RolePermissionComponent,
        UpdateRoleUsersComponent,
    ],
    imports: [
        CommonModule,
        RolesRoutingModule,
        CardModule,
        ButtonModule,
        ReactiveFormsModule,
        FormsModule,
        InputTextModule,
        PanelModule,
        UtilityModule,
    ],
    providers: [MessageService, PrimeNGConfig],
})
export class RolesModule {}
