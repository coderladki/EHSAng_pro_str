import { Component } from '@angular/core';

@Component({
  selector: 'app-workpermitdashboard',
  templateUrl: './workpermitdashboard.component.html',
  styleUrls: ['./workpermitdashboard.component.scss']
})
export class WorkpermitdashboardComponent {
  date: Date;
  displayBasic: boolean;
  showBasicDialog() {
    this.displayBasic = true;
  }

}
