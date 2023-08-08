import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UtilityRoutingModule } from './utility-routing.module';
import { ToastComponent } from './toast/toast.component';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { UtilityService } from './utility.service';
import { SpinnerComponent } from './spinner/spinner.component';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';

@NgModule({
    declarations: [SpinnerComponent, ToastComponent],
    exports: [SpinnerComponent, ToastComponent],
    imports: [
        CommonModule,
        UtilityRoutingModule,
        ProgressSpinnerModule,
        MessagesModule,
        MessageModule,
        ToastModule,
    ],
    providers: [UtilityService],
})
export class UtilityModule {}
