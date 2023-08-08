import { Injectable, OnDestroy } from '@angular/core';
import { Observable, BehaviorSubject, of, Subscription } from 'rxjs';
import { map, catchError, switchMap, finalize } from 'rxjs/operators';
import { UserModel } from '../../models/auth/user.model';
import { AuthModel } from '../../models/auth/auth.model';
import { AuthHTTPService } from '../../api/services/auth-http.service';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
export type UserType = UserModel | undefined;
@Injectable({
    providedIn: 'root',
})
export class AuthService implements OnDestroy {
    // private fields
    private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/
    private authLocalStorageToken = `${environment.appVersion}-${environment.USERDATA_KEY}`;
    private suAuthLocalStorageToken = `${environment.appVersion}-${environment.PANTUSERDATA_KEY}`;
    // public fields
    currentUser$: Observable<UserType>;
    isLoading$: Observable<boolean>;
    currentUserSubject: BehaviorSubject<UserType>;
    isLoadingSubject: BehaviorSubject<boolean>;

    get currentUserValue(): UserType {
        return this.currentUserSubject.value;
    }

    set currentUserValue(user: UserType) {
        this.currentUserSubject.next(user);
    }

    constructor(
        private authHttpService: AuthHTTPService,
        private router: Router
    ) {
        this.isLoadingSubject = new BehaviorSubject<boolean>(false);
        this.currentUserSubject = new BehaviorSubject<UserType>(undefined);
        this.currentUser$ = this.currentUserSubject.asObservable();
        this.isLoading$ = this.isLoadingSubject.asObservable();
        const subscr = this.getUserByToken().subscribe();
        this.unsubscribe.push(subscr);
    }

    // public methods
    login(
        userName: string,
        password: string,
        isPlantAdminLogin: boolean = false
    ): Observable<UserType> {
        this.isLoadingSubject.next(true);
        return this.authHttpService.login(userName, password).pipe(
            map((loginResponse: any) => {
                if (isPlantAdminLogin) {
                    this.impersonationProcess(isPlantAdminLogin);
                }
                let auth = loginResponse.token as AuthModel;
                const result = this.setAuthFromLocalStorage(auth);

                this.setUserPermission(
                    loginResponse.id,
                    loginResponse.result.permissions
                );
                localStorage.setItem(
                    'User Permissions',
                    JSON.stringify(loginResponse.result.permissions)
                );
                localStorage.setItem(
                    'User Navigation Permission',
                    JSON.stringify(loginResponse.result.navigationPermissions)
                );
                this.getUserByToken();
            }),
            switchMap(() => this.getUserByToken()),
            catchError((err) => {
                return of(undefined);
            }),
            finalize(() => this.isLoadingSubject.next(false))
        );
    }

    setUserPermission(id: number, data: any): Observable<boolean> {
        localStorage.setItem('User Permissions', JSON.stringify(data));
        return new Observable((observer) => {
            observer.next(true);
            observer.complete();
        });
    }
    refreshUserPermission(): Observable<boolean> {
        const auth = this.getAuthFromLocalStorage();
        if (auth != undefined && this.currentUserValue != undefined) {
            return new Observable((observer) => {
                if (this.currentUserValue != undefined) {
                    this.authHttpService
                        .refreshUserPermissions(
                            auth.access_token,
                            this.currentUserValue.id
                        )
                        .subscribe(
                            (res) => {
                                observer.next(true);
                            },
                            (error) => {
                                observer.next(false);
                            }
                        );
                }
            });
        } else {
            return new Observable((observer) => {
                observer.next(false);
            });
        }
    }

    logout() {
        let isPlantAdminLogin = localStorage.getItem('isPlantAdminLogin');
        if (isPlantAdminLogin?.toLowerCase() == 'true') {
            this.impersonationProcess(false);
            localStorage.removeItem('isPlantAdminLogin');
            localStorage.removeItem(this.suAuthLocalStorageToken);
            this.router.navigateByUrl('/master/unit');
        } else {
            localStorage.removeItem(this.authLocalStorageToken);
            this.router.navigate(['/auth/login'], {
                queryParams: {},
            });
        }
    }

    impersonationProcess(isPlantAdminLogin: boolean) {
        if (isPlantAdminLogin) {
            localStorage.setItem(
                'isPlantAdminLogin',
                isPlantAdminLogin.toString()
            );
            var suAdminData =
                localStorage.getItem(this.authLocalStorageToken) ?? '';
            localStorage.setItem(this.suAuthLocalStorageToken, suAdminData);
            localStorage.removeItem(this.authLocalStorageToken);

            var suPermissiondata =
                localStorage.getItem('User Permissions') ?? '';
            localStorage.setItem('SuperAdminUserPermission', suPermissiondata);
            localStorage.removeItem('User Permissions');

            var suNavPermissionData =
                localStorage.getItem('User Navigation Permission') ?? '';
            localStorage.setItem(
                'SuperAdminUserNavigationPermission',
                suNavPermissionData
            );
            localStorage.removeItem('User Navigation Permission');
        } else {
            localStorage.setItem(
                'isPlantAdminLogin',
                isPlantAdminLogin.toString()
            );
            var adminData =
                localStorage.getItem(this.suAuthLocalStorageToken) ?? '';
            localStorage.setItem(this.authLocalStorageToken, adminData);
            localStorage.removeItem(this.suAuthLocalStorageToken);

            var userPermissionData =
                localStorage.getItem('SuperAdminUserPermission') ?? '';
            localStorage.setItem('User Permissions', userPermissionData);
            localStorage.removeItem('SuperAdminUserPermission');

            var userNavPermissionData =
                localStorage.getItem('SuperAdminUserNavigationPermission') ??
                '';
            localStorage.setItem(
                'User Navigation Permission',
                userNavPermissionData
            );
            localStorage.removeItem('SuperAdminUserNavigationPermission');
        }
    }

    getUserByToken(): Observable<UserType> {
        const auth = this.getAuthFromLocalStorage();
        if (!auth || !auth.access_token) {
            return of(undefined);
        }
        this.isLoadingSubject.next(true);
        return this.authHttpService.getUserByToken(auth.access_token).pipe(
            map((user: any) => {
                if (user) {
                    this.currentUserSubject.next(user);
                } else {
                    this.logout();
                }
                return user;
            }),
            finalize(() => this.isLoadingSubject.next(false))
        );
    }

    // need create new user then login
    registration(user: UserModel): Observable<any> {
        this.isLoadingSubject.next(true);
        return this.authHttpService.createUser(user).pipe(
            map(() => {
                this.isLoadingSubject.next(false);
            }),
            switchMap(() => this.login(user.email, user.password)),
            catchError((err) => {
                console.error('err', err);
                return of(undefined);
            }),
            finalize(() => this.isLoadingSubject.next(false))
        );
    }

    forgotPassword(email: string): Observable<boolean> {
        this.isLoadingSubject.next(true);
        return this.authHttpService
            .forgotPassword(email)
            .pipe(finalize(() => this.isLoadingSubject.next(false)));
    }

    // private methods
    private setAuthFromLocalStorage(auth: AuthModel): boolean {
        // store auth authToken/refreshToken/epiresIn in local storage to keep user logged in between page refreshes

        if (auth && auth.access_token) {
            localStorage.setItem(
                this.authLocalStorageToken,
                JSON.stringify(auth)
            );
            return true;
        } else {
            return false;
        }
    }

    public getAuthFromLocalStorage(): AuthModel | undefined {
        try {
            const lsValue = localStorage.getItem(this.authLocalStorageToken);
            if (!lsValue) {
                return undefined;
            }

            const authData = JSON.parse(lsValue);
            return authData;
        } catch (error) {
            console.error(error);
            return undefined;
        }
    }

    ngOnDestroy() {
        this.unsubscribe.forEach((sb) => sb.unsubscribe());
    }
}
