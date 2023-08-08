import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { LayoutService } from './service/app.layout.service';
import { map, Observable, switchMap } from 'rxjs';
import { MenuModel } from 'src/app/interfaces/Menu.class';
import { AuthService } from '../services/auth/auth.service';
import { environment } from '../../environments/environment';
import { MasterService } from '../services/masters/master.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {
    model: any[] = [];
    appAngularVersion: string = environment.appVersion;
    appPreviewChangelogUrl: string = environment.appPreviewChangelogUrl;

    ParentMenuList: MenuModel[];
    user$: Observable<any>;
    _user: any;

    constructor(
        public layoutService: LayoutService,
        private masterService: MasterService,
        private cdRef: ChangeDetectorRef,
        private auth: AuthService
    ) { }

    ngOnInit() {
        this.user$ = this.auth.currentUserSubject.asObservable();
        this.user$.subscribe((res) => {
            this._user = res;
        });
        //this.getDashboardMenu();
        this.getDashboardMenu2();
    }
    getDashboardMenu() {
        this.masterService
            .masterGetMethod(`/DashboardMenus/getall`)
            .subscribe((res) => {
                this.ParentMenuList = res.filter(
                    (x: MenuModel) => x.parent == null
                );
                for (let pm of this.ParentMenuList) {
                    if (pm.children == undefined) {
                        pm.children = [];
                    }
                    //console.log(this.ParentMenuList);
                    var childMenus = [];
                    let _label = '';
                    if (pm.title.toLowerCase() == 'dashboard') {
                        _label = 'Home';
                    } else {
                        _label = pm.title;
                    }
                    for (let r of res) {
                        if (r.parent == pm.id) {
                            r.url =
                                '/' +
                                pm.actualpath.replace(/ /g, '-').toLowerCase() +
                                '/' +
                                r.actualpath
                                    .replaceAll(' ', '-')
                                    .toLowerCase() +
                                '/';
                            pm.children.push(r);
                            childMenus.push({
                                label: r.title,
                                icon: 'pi pi-fw pi-home',
                                routerLink: [r.url],
                            });
                        }
                    }
                    var _model = {
                        //label: _label,//here Group menu label
                        items: [
                            {
                                label: pm.title,
                                icon: 'pi pi-fw pi-user',
                                items: childMenus,
                            },
                        ],
                    };
                    this.model.push(_model);
                }
                this.cdRef.detectChanges();
            });
    }

    getDashboardMenu2() {
        this.masterService
            .masterGetMethod(`/DashboardMenus/getall`)
            .subscribe((res) => {
                this.ParentMenuList = res.filter(
                    (x: MenuModel) => x.parent == null
                );
                for (let pm of this.ParentMenuList) {
                    this.model.push(this.recursiveMenuGenerator(res, pm));
                }
                this.cdRef.detectChanges();
            });
    }
    recursiveMenuGenerator(actualList: any, currentParent: any) {
        if (currentParent.children == undefined) {
            currentParent.children = [];
        }
        //console.log(this.ParentMenuList);
        var childMenus = [];
        let _label = '';
        if (currentParent.title.toLowerCase() == 'dashboard') {
            _label = 'Home';
        } else {
            _label = currentParent.title;
        }
        for (let r of actualList) {
            if (r.parent == currentParent.id) {
                if (r.actualpath) {
                    r.url =
                        '/' +
                        currentParent.actualpath.replace(/ /g, '-').toLowerCase() +
                        '/' +
                        r.actualpath
                            .replaceAll(' ', '-')
                            .toLowerCase() +
                        '/';
                    currentParent.children.push(r);
                    childMenus.push({
                        label: r.title,
                        icon: 'pi pi-fw pi-home',
                        routerLink: [r.url],
                    });
                }
                else {
                    childMenus.push(this.recursiveMenuGenerator(actualList, r));
                }
            }
        }
        var _model: any = {
            label: currentParent.title,
            icon: 'pi pi-fw pi-user',
            items: childMenus,

        };
        return _model;

    }
}
