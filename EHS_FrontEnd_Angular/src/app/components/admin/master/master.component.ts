import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
    selector: 'app-master',
    template: '<router-outlet></router-outlet><app-spinner><app-spinner>',
})
export class MasterComponent implements OnInit, OnDestroy {
    ngOnDestroy(): void {
        //throw new Error('Method not implemented.');
    }
    ngOnInit(): void {
        //throw new Error('Method not implemented.');
    }
}
