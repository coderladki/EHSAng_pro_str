import { Injectable } from '@angular/core';
import { observable, Observable, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserModel } from '../../models/auth/user.model';
import { environment } from '../../../environments/environment';
import { AuthModel } from '../../models/auth/auth.model';
const API_USERS_URL = `${environment.apiUrl}/auth`;
@Injectable({
    providedIn: 'root',
})
export class AuthHTTPService {
    constructor(private http: HttpClient) {}

    // public methods
    login(userName: string, password: string): Observable<any> {
        return this.http.post<any>(`${API_USERS_URL}/login`, {
            userName,
            password,
        });
    }

    // CREATE =>  POST: add a new user to the server
    createUser(user: any): Observable<any> {
        let returnSubject: Subject<any> = new Subject();
        this.http
            .post<any>(environment.apiUrl + '/users/create', user)
            .subscribe((res: any) => {
                // If response comes hideloader() function is called
                // to hide that loader
                if (res) {
                    returnSubject.next(res);
                    returnSubject.unsubscribe();
                } else {
                }
            });
        return returnSubject;
    }

    // Your server should check email => If email exists send link to the user and return true | If email doesn't exist return false
    forgotPassword(email: string): Observable<boolean> {
        return this.http.post<boolean>(`${API_USERS_URL}/forgot-password`, {
            email,
        });
    }

    getUserByToken(token: string): Observable<any> {
        const httpHeaders = new HttpHeaders({
            Authorization: `Bearer ${token}`,
        });
        return this.http.get<any>(`${environment.apiUrl}/users/loggeduser`, {
            headers: httpHeaders,
        });
    }
    refreshUserPermissions(token: string, UserId: number): Observable<boolean> {
        let getUrl = `/Permissions/GetAllUserPermissions/${UserId}`;
        let reqHeader = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': environment.apiUrl,
            Authorization: `Bearer ${token}`,
        });
        //let params = new HttpParams()
        return new Observable((observer) => {
            this.http
                .get<any>(environment.apiUrl + getUrl, {
                    headers: reqHeader,
                })
                .subscribe(
                    (res) => {
                        localStorage.setItem(
                            'User Permission',
                            JSON.stringify(res)
                        );
                        observer.next(true);
                        observer.complete();
                    },
                    (error) => {
                        localStorage.removeItem('User Permission');
                        observer.next(false);
                        observer.complete();
                    },
                    () => {
                        observer.complete();
                    }
                );
        });
    }
}
