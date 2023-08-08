import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EhsRoutingModule } from './Ehs-routing.module';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { MultiSelectModule } from 'primeng/multiselect';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { EhsComponent } from './Ehs.component';
import { DeptandsectionsComponent } from './deptandsections/deptandsections.component';
import { DepartmentsubsectioneditComponent } from './departmentsubsectionedit/departmentsubsectionedit.component';


@NgModule({
  declarations: [
    EhsComponent,
    DeptandsectionsComponent,
    DepartmentsubsectioneditComponent

  ],
  imports: [
    FormsModule,
    ButtonModule,
    CommonModule,
    EhsRoutingModule,
    CalendarModule,
    MultiSelectModule,
    DialogModule,
    TableModule,
  ]
})
export class EhsModule { }
