import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersRoutingModule } from './users-routing.module';
import { UsersComponent } from './users.component';
import { UserListComponent } from './user-list/user-list.component';
import { UtilityModule } from 'src/app/utility/utility.module';
import { AddUserComponent } from './add-user/add-user.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditUserComponent } from './edit-user/edit-user.component';
import { MultiSelectModule } from 'primeng/multiselect';
import { TableModule } from 'primeng/table';
import { MessageService, PrimeNGConfig } from 'primeng/api';

@NgModule({
    declarations: [
        UsersComponent,
        UserListComponent,
        AddUserComponent,
        EditUserComponent,
    ],
    imports: [
        CommonModule,
        UsersRoutingModule,
        TableModule,
        UtilityModule,
        ReactiveFormsModule,
        FormsModule,
        MultiSelectModule,
    ],
    providers: [MessageService, PrimeNGConfig],
})
export class UsersModule {}
