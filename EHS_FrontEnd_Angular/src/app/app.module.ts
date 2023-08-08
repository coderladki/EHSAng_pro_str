import { NgModule, APP_INITIALIZER } from '@angular/core';
import {
    DatePipe,
    HashLocationStrategy,
    LocationStrategy,
} from '@angular/common';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppLayoutModule } from './layout/app.layout.module';
import { AuthService } from '../app/services/auth/auth.service';
import { environment } from '../environments/environment';
import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
import { FakeAPIService } from '../app/services/FakeAPI/fake-api.service';
import { ToastModule } from 'primeng/toast';
import { UtilityModule } from './utility/utility.module';
import { BaseComponent } from './base/base.component';
import { WorkpermittypedetailComponent } from './components/admin/master/workpermittypedetail/workpermittypedetail.component';

// import { ProductService } from './service/product.service';
// import { CountryService } from './service/country.service';
// import { CustomerService } from './service/customer.service';
// import { EventService } from './service/event.service';
// import { IconService } from './service/icon.service';
// import { NodeService } from './service/node.service';
// import { PhotoService } from './service/photo.service';
function appInitializer(authService: AuthService) {
    return () => {
        return new Promise((resolve) =>
            setTimeout(
                () => resolve(authService.getUserByToken().subscribe()),
                500
            )
        );
    };
}
@NgModule({
    declarations: [AppComponent],
    imports: [
        AppRoutingModule,
        AppLayoutModule,
        ToastModule,
        environment.isMockEnabled
            ? HttpClientInMemoryWebApiModule.forRoot(FakeAPIService, {
                passThruUnknownUrl: true,
                dataEncapsulation: false,
            })
            : [],
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        // CountryService,
        // CustomerService,
        // EventService,
        // IconService,
        // NodeService,
        // PhotoService,
        // ProductService,
        {
            provide: APP_INITIALIZER,
            useFactory: appInitializer,
            multi: true,
            deps: [AuthService],
        },
        [DatePipe],
    ],

    bootstrap: [AppComponent],
})
export class AppModule { }
