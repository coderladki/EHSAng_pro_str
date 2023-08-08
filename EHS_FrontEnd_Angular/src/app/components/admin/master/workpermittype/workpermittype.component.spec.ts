import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkpermittypeComponent } from './workpermittype.component';

describe('WorkpermittypeComponent', () => {
  let component: WorkpermittypeComponent;
  let fixture: ComponentFixture<WorkpermittypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WorkpermittypeComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(WorkpermittypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});