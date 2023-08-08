import { Injector } from '@angular/core';

export abstract class BaseComponent {
    constructor(injector: Injector) {}
    validateText(event: any) {
        const pattern = /[A-Za-z\s]/g;
        const keyCode = event.keyCode;
        const excludedKeys = [8, 37, 39, 46];
        if (!event.key.match(pattern) || excludedKeys.includes(keyCode)) {
            event.preventDefault();
        }
    }

    validateNumber(event: any) {
        const pattern = /[0-9]/g;
        const keyCode = event.keyCode;
        const excludedKeys = [8, 37, 39, 46];
        if (!event.key.match(pattern) || excludedKeys.includes(keyCode)) {
            event.preventDefault();
        }
    }
}
