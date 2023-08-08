import { Injectable } from '@angular/core';
import { Observable, Subject, throwError } from 'rxjs';
import {
    HttpClient,
    HttpErrorResponse,
    HttpHeaders,
    HttpParams,
    HttpResponseBase,
} from '@angular/common/http';
import { catchError } from 'rxjs/operators';
//import * as moment from 'moment';
import { environment } from 'src/environments/environment';
import { AuthService } from '../auth/auth.service';

@Injectable({
    providedIn: 'root',
})
export class MasterService {
    public UserList: any[] = [];
    dateFilterRange: any = {
        // Today: [Date(), moment()],
        // Yesterday: [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        // 'Last 7 Days': [moment().subtract(6, 'days'), moment()],
        // 'Last 30 Days': [moment().subtract(29, 'days'), moment()],
        // 'This Month': [moment().startOf('month'), moment().endOf('month')],
        // 'Last Month': [
        //     moment().subtract(1, 'month').startOf('month'),
        //     moment().subtract(1, 'month').endOf('month'),
        // ],
    };
    accessToken: string;
    transformDate(date: string) {
        //return moment(date).format('YYYY/MM/DD');
    }

    error: any;
    apiBaseURL: string = environment.apiUrl;
    headers = new HttpHeaders().set('Content-Type', 'application/json');
    reqHeader: HttpHeaders;
    public readonly BaseUri: string = environment.apiUrl;
    constructor(
        private _httpclient: HttpClient,
        private _authService: AuthService
    ) {
        var result = this._authService.getAuthFromLocalStorage();
        this.accessToken = result == undefined ? '' : result.access_token;
        this.reqHeader = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': this.BaseUri,
            Authorization: `Bearer ${this.accessToken}`,
        });
    }

    masterPostFileMethod(
        posturl: string,
        requestData: FormData
    ): Observable<any> {
        const reqHeader = new HttpHeaders({
            'Access-Control-Allow-Origin': this.BaseUri,
            Authorization: `Bearer ${this.accessToken}`,
        });
        let params = new HttpParams();
        return this._httpclient
            .post<any>(this.BaseUri + posturl, requestData, {
                headers: reqHeader,
                params: params,
            })
            .pipe(catchError(this.handleError));
    }

    masterPostMethod(posturl: string, requestData: any): Observable<any> {
        let params = new HttpParams();
        var data = JSON.stringify(requestData);
        return this._httpclient
            .post<any>(this.BaseUri + posturl, data, {
                headers: this.reqHeader,
                params: params,
            })
            .pipe(catchError(this.handleError));
    }

    masterGetMethod(getUrl: string): Observable<any> {
        let reqHeader = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': this.BaseUri,
        });
        //let params = new HttpParams()
        var response: Observable<HttpResponseBase | HttpErrorResponse>;
        return this._httpclient
            .get<HttpResponseBase>(this.BaseUri + getUrl, {
                headers: this.reqHeader,
            })
            .pipe(catchError(this.handleError));
    }

    public handleError(
        error: HttpErrorResponse
    ): Observable<HttpErrorResponse> {
        return new Observable((observer) => {
            if (error.ok == false) {
                if (error.error != null) {
                    //alert(error.error)

                    observer.error(error.error);
                } else {
                    alert(error.statusText);
                    observer.error(error.statusText);
                }
            } else {
                observer.error(error.statusText);
            }
        });
    }

    masterPostFormDataMethod(
        posturl: string,
        requestData: FormData
    ): Observable<any> {
        const reqHeader = new HttpHeaders({
            'Access-Control-Allow-Origin': this.BaseUri,
            Authorization: `Bearer ${this.accessToken}`,
        });
        let params = new HttpParams();
        return this._httpclient
            .post<any>(this.BaseUri + posturl, requestData, {
                headers: reqHeader,
                params: params,
            })
            .pipe(catchError(this.handleError));
    }
}
