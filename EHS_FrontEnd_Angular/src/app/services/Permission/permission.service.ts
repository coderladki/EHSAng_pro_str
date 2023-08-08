import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Permission } from '../../interfaces/Permission.class';
import { EHSPermissionDefination } from '../../models/enums/EHSPermissionEnum';
import { AuthService } from '../../services/auth/auth.service';
import { MasterService } from '../../services/masters/master.service';

@Injectable({
    providedIn: 'root',
})
export class PermissionService {
    constructor(
        private masterService: MasterService,
        private authService: AuthService
    ) {}
    Permitted(route: string) {
        var lsValue = localStorage.getItem('User Permissions');
        if (lsValue != null) {
            var userPermissions = JSON.parse(lsValue);
            for (let permission of userPermissions) {
                if (route == permission) {
                    return true;
                }
            }
            return false;
        } else {
            this.authService.logout();
            return false;
        }
    }
    getAllPermissions = new Observable<Permission[]>((observer) => {
        this.masterService.masterGetMethod(`/Permissions/GetAll`).subscribe(
            (res) => {
                observer.next(res.result);
                observer.complete();
            },
            (error) => {
                alert(error);
                observer.error(error);
                observer.complete();
            }
        );
    });
    getAllPermissionsAssignedToRole(RoleId: Number): Observable<string[]> {
        return new Observable<string[]>((observer) => {
            this.masterService
                .masterGetMethod(`/Permissions/GetAllRolePermissions/${RoleId}`)
                .subscribe(
                    (res) => {
                        observer.next(res.result);
                        observer.complete();
                    },
                    (error) => {
                        alert(error);
                        observer.error([]);
                        observer.complete();
                    }
                );
        });
    }

    getAllNavigationPermissionsAssignedToRole(
        RoleId: Number
    ): Observable<string[]> {
        return new Observable<string[]>((observer) => {
            this.masterService
                .masterGetMethod(
                    `/Permissions/GetAllNavigationPermissions/${RoleId}`
                )
                .subscribe(
                    (res) => {
                        observer.next(res.result);
                        observer.complete();
                    },
                    (error) => {
                        alert(error);
                        observer.error([]);
                        observer.complete();
                    }
                );
        });
    }

    getAllNavigations = new Observable<Permission[]>((observer) => {
        this.masterService
            .masterGetMethod(`/Permissions/GetAllNavigation`)
            .subscribe(
                (res) => {
                    observer.next(res.result);
                    observer.complete();
                },
                (error) => {
                    alert(error);
                    observer.error(error);
                    observer.complete();
                }
            );
    });

    getAllPermissionsAssignedToUser(UserId: Number): Observable<string[]> {
        return new Observable<string[]>((observer) => {
            this.masterService
                .masterGetMethod(`/Permissions/GetAllUserPermissions/${UserId}`)
                .subscribe(
                    (res) => {
                        observer.next(res.result);
                        observer.complete();
                    },
                    (error) => {
                        alert(error);
                        observer.error([]);
                        observer.complete();
                    }
                );
        });
    }

    getAllNavigationPermissionsAssignedToUser(
        UserId: Number
    ): Observable<string[]> {
        return new Observable<string[]>((observer) => {
            this.masterService
                .masterGetMethod(
                    `/Permissions/GetAllUserNavigationPermissions/${UserId}`
                )
                .subscribe(
                    (res) => {
                        observer.next(res.result);
                        observer.complete();
                    },
                    (error) => {
                        alert(error);
                        observer.error([]);
                        observer.complete();
                    }
                );
        });
    }

    hasPermission(crmPermission: EHSPermissionDefination) {
        var lsValue = localStorage.getItem('User Permissions');
        if (lsValue != null) {
            var userPermissions = JSON.parse(lsValue);
            for (let permission of userPermissions) {
                if (crmPermission == permission) {
                    return true;
                }
            }
            return false;
        } else {
            return false;
        }
    }
}
