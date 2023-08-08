import { ChangeDetectorRef, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MasterService } from '../../services/masters/master.service';

@Injectable({
    providedIn: 'root',
})
export class RoleService {
    RoleList: Observable<any>;
    RoleRights: Observable<any>;
    constructor(private masterService: MasterService) {
        this.RoleList = new Observable((observer) => {
            this.masterService.masterGetMethod(`/roles/list`).subscribe(
                (res) => {
                    observer.next(res);
                },
                (error) => {
                    observer.error(error);
                }
            );
        });
    }
}
