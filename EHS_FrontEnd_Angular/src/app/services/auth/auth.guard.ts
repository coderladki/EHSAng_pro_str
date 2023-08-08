import { Injectable } from '@angular/core';
import {
    CanActivate,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
} from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard implements CanActivate {
    constructor(private authService: AuthService) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.authService.currentUserValue;
        if (currentUser) {
            try {
                var lsValue = localStorage.getItem('User Permissions');
                if (!lsValue) {
                    this.authService
                        .refreshUserPermission()
                        .subscribe((res) => {
                            if (res == true) {
                                lsValue =
                                    localStorage.getItem('User Permissions');
                                if (lsValue) {
                                    return this.UserPermission(lsValue, route);
                                } else {
                                    return false;
                                }
                            } else {
                                return false;
                            }
                        });
                } else {
                    let response = this.UserPermission(lsValue, route);
                    return response;
                }
            } catch (error) {
                return false;
            }
            // return true;
        }

        // not logged in so redirect to login page with the return url
        this.authService.logout();
        return false;
    }

    UserPermission(lsValue: string, route: ActivatedRouteSnapshot) {
        //return true;
        try {
            if (lsValue) {
                let UserPermissions = JSON.parse(lsValue);
                if (route.url == undefined || route.url.length == 0) {
                    return true;
                }
                if (route.url[0]?.path != undefined) {
                    let currentpath = route.url[0]?.path;
                    for (let permission of UserPermissions) {
                        if (currentpath == permission) {
                            return true;
                        }
                    }
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        } catch (error) {
            return false;
        }
    }
}
