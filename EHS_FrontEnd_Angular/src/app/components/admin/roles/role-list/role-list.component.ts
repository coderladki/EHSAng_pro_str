import {
    Component,
    OnInit,
    AfterViewInit,
    ChangeDetectorRef,
    OnDestroy,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MasterService } from '../../../../services/masters/master.service';

@Component({
    selector: 'app-role-list',
    templateUrl: './role-list.component.html',
    styleUrls: ['./role-list.component.scss'],
})
export class RoleListComponent implements OnInit, OnDestroy, AfterViewInit {
    rolelist: any[] = [];
    rolelistfilter: any[];
    private unsubscribe: Subscription[] = [];
    private NavigateUrl: string = '/roles/add-role';
    constructor(
        private masterService: MasterService,
        private cdRef: ChangeDetectorRef,
        private aroute: ActivatedRoute,
        private router: Router
    ) {}
    ngAfterViewInit(): void {
        const _roleList = this.masterService
            .masterGetMethod('/roles/list')
            .subscribe(
                (res) => {
                    this.rolelist = res;
                    this.rolelistfilter = res;
                    this.cdRef.detectChanges();
                },
                (error) => {
                    this.rolelist = [];
                }
            );

        this.unsubscribe.push(_roleList);
    }

    ngOnInit(): void {
        //throw new Error('Method not implemented.');
    }
    onEdit(id: any) {
        this.router.navigate([this.NavigateUrl], {
            queryParams: { roleId: id, mode: 'edit' },
        });
    }
    onDelete(id: any) {
        this.router.navigate([this.NavigateUrl], {
            queryParams: { roleId: id, mode: 'delete' },
        });
    }
    onPermission(id: any) {
        this.router.navigate(['/roles/role-permission'], {
            queryParams: { roleId: id },
        });
    }
    ngOnDestroy(): void {
        this.unsubscribe.forEach((sb) => sb.unsubscribe());
    }
}
