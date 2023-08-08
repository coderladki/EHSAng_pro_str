import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ErrorComponent } from '../../auth/error/error.component';
import { DepartmentsubsectioneditComponent } from './departmentsubsectionedit/departmentsubsectionedit.component';
import { DeptandsectionsComponent } from './deptandsections/deptandsections.component';
import { EhsComponent } from './Ehs.component';


const routes: Routes = [
  {
    path: '',
    component: EhsComponent,
    children: [
      {
        path: 'deptandsections',
        component: DeptandsectionsComponent
      },
      {
        path: 'dept-subsection-edit',
        component: DepartmentsubsectioneditComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EhsRoutingModule { }