import { Component, OnInit } from '@angular/core';
import { UtilityService } from '../utility.service';

@Component({
    selector: 'app-spinner',
    templateUrl: './spinner.component.html',
    styleUrls: ['./spinner.component.scss'],
})
export class SpinnerComponent implements OnInit {
    isSpinnerShow: boolean = false;
    isSpinnerText: string = '';
    constructor(private utiltity: UtilityService) {}
    ngOnInit(): void {
        this.utiltity.castSpinner.subscribe(
            (data) => (this.isSpinnerShow = data)
        );
        this.utiltity.message.subscribe((data) => (this.isSpinnerText = data));
    }
}
