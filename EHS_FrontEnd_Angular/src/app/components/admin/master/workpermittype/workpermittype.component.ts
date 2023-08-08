import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-workpermittype',
  templateUrl: './workpermittype.component.html',
  styleUrls: ['./workpermittype.component.scss']
})
export class WorkpermittypeComponent {

  constructor(private router: Router, private route: ActivatedRoute) {

  }
  goTotype(typeName: string) {
    this.router.navigate([`../workpermittypedetail/${typeName}`], { relativeTo: this.route })
  }
}