import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorComponent } from '../../auth/error/error.component';
import { DivisionMasterComponent } from './division-master/division-master.component';
import { EmailMasterComponent } from './email-master/email-master.component';
import { EmployeeMasterComponent } from './employee-master/employee-master.component';
import { IncidentComponent } from './incident/incident.component';
import { MasterComponent } from './master.component';
import { UnitMasterComponent } from './unit-master/unit-master.component';
import { WorkpermitdashboardComponent } from './workpermitdashboard/workpermitdashboard.component';
import { WorkpermittypeComponent } from './workpermittype/workpermittype.component';
import { WorkpermittypedetailComponent } from './workpermittypedetail/workpermittypedetail.component';

const routes: Routes = [
  {
    path: '',
    component: MasterComponent,
    children: [
      {
        path: 'division',
        component: DivisionMasterComponent
      },
      {
        path: 'employee',
        component: EmployeeMasterComponent
      },
      {
        path: 'email',
        component: EmailMasterComponent
      },
      {
        path: 'unit',
        component: UnitMasterComponent
      },
      {
        path: 'incident',
        component: IncidentComponent
      },
      {
        path: 'workpermitdashboard',
        component: WorkpermitdashboardComponent
      },
      {
        path: 'workpermittype',
        component: WorkpermittypeComponent
      },
      {
        path: 'workpermittypedetail/:TypeName',
        component: WorkpermittypedetailComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MasterRoutingModule { }