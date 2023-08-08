import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppLayoutComponent } from './layout/app.layout.component';
import { ErrorComponent } from './components/auth/error/error.component';
import { AuthGuard } from './services/auth/auth.guard';
import { MasterModule } from '../app/components/admin/master/master.module';

@NgModule({
    imports: [
        RouterModule.forRoot(
            [
                {
                    path: '',
                    component: AppLayoutComponent,
                    canActivate: [AuthGuard],
                    children: [
                        {
                            path: 'dashboard',
                            loadChildren: () =>
                                import(
                                    '../app/components/admin/dashboard/dashboard.module'
                                ).then((m) => m.DashboardModule),
                        },
                        {
                            path: 'roles',
                            loadChildren: () =>
                                import(
                                    '../app/components/admin/roles/roles.module'
                                ).then((m) => m.RolesModule),
                        },
                        {
                            path: 'master',
                            loadChildren: () =>
                                import(
                                    '../app/components/admin/master/master.module'
                                ).then((m) => m.MasterModule),
                        },

                        {
                            path: 'ehs',
                            loadChildren: () =>
                                import(
                                    '../app/components/admin/EhsModule/Ehs.module'
                                ).then((m) => m.EhsModule),
                        },
                        {
                            path: 'users',
                            loadChildren: () =>
                                import(
                                    '../app/components/admin/users/users.module'
                                ).then((m) => m.UsersModule),
                        },
                    ],
                },
                {
                    path: 'auth',
                    loadChildren: () =>
                        import('./components/auth/auth.module').then(
                            (m) => m.AuthModule
                        ),
                },
                { path: '**', component: ErrorComponent },
            ],
            {
                scrollPositionRestoration: 'enabled',
                anchorScrolling: 'enabled',
                onSameUrlNavigation: 'reload',
            }
        ),
    ],
    exports: [RouterModule],
})
export class AppRoutingModule { }
