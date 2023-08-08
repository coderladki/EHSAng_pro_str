import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterRoutingModule } from './master-routing.module';
import { DivisionMasterComponent } from './division-master/division-master.component';
import { MasterComponent } from '../master/master.component';
import { EmployeeMasterComponent } from './employee-master/employee-master.component';
import { EmailMasterComponent } from './email-master/email-master.component';
import { UnitMasterComponent } from './unit-master/unit-master.component';
import { IncidentComponent } from './incident/incident.component';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { MultiSelectModule } from 'primeng/multiselect';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { WorkpermitdashboardComponent } from './workpermitdashboard/workpermitdashboard.component';
import { UtilityModule } from 'src/app/utility/utility.module';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { DividerModule } from 'primeng/divider';
import { WorkpermittypedetailComponent } from './workpermittypedetail/workpermittypedetail.component';

@NgModule({
  declarations: [
    DivisionMasterComponent,
    MasterComponent,
    EmployeeMasterComponent,
    EmailMasterComponent,
    UnitMasterComponent,
    IncidentComponent,
    WorkpermitdashboardComponent,
    WorkpermittypedetailComponent
  ],
  imports: [
    FormsModule,
    ButtonModule,
    CommonModule,
    MasterRoutingModule,
    CalendarModule,
    DividerModule,
    MultiSelectModule,
    DialogModule,
    TableModule,
    UtilityModule,
  ],
  providers: [MessageService, PrimeNGConfig],
})
export class MasterModule { }
