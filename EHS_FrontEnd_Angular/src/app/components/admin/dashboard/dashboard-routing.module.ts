import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { PlantAdminComponent } from './plant-admin/plant-admin.component';
import { SuperadminComponent } from './superadmin/superadmin.component';

const routes: Routes = [
    {
        path: '',
        component: DashboardComponent,
    },
    {
        path: 'super-admin',
        component: SuperadminComponent,
    },
    {
        path: 'plant-admin',
        component: PlantAdminComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DashboardRoutingModule {}
