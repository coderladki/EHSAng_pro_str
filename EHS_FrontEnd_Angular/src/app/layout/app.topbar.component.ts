import {
    Component,
    ElementRef,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MenuItem } from 'primeng/api';
import { AuthService } from '../services/auth/auth.service';
import { LayoutService } from './service/app.layout.service';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html',
})
export class AppTopBarComponent implements OnInit, OnDestroy {
    items!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    constructor(
        public layoutService: LayoutService,
        private auth: AuthService
    ) {}

    ngOnInit(): void {
        //throw new Error('Method not implemented.');
    }
    logout() {
        this.auth.logout();
        document.location.reload();
    }

    ngOnDestroy(): void {
        //throw new Error('Method not implemented.');
    }
}
