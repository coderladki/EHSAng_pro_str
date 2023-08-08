import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { SuperadminComponent } from './superadmin/superadmin.component';
import { PlantAdminComponent } from './plant-admin/plant-admin.component';

@NgModule({
    declarations: [DashboardComponent, SuperadminComponent, PlantAdminComponent],
    imports: [CommonModule, DashboardRoutingModule],
})
export class DashboardModule {}
