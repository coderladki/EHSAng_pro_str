import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { UtilityService } from '../utility.service';

@Component({
    selector: 'app-toast',
    templateUrl: './toast.component.html',
})
export class ToastComponent implements OnInit {
    _severity: string = '';
    _summary: string = '';
    _detail: string = '';
    constructor() {}
    ngOnInit(): void {}
    // @Output() Reject: EventEmitter<boolean> = new EventEmitter<boolean>();
    // @Output() Confirm: EventEmitter<boolean> = new EventEmitter<boolean>();
    // onReject() {
    //     this.Reject.emit(false);
    // }
    // onConfirm() {
    //     this.Confirm.emit(true);
    // }
}
