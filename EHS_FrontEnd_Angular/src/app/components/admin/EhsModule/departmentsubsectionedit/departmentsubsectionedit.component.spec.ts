import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentsubsectioneditComponent } from './departmentsubsectionedit.component';

describe('DepartmentsubsectioneditComponent', () => {
  let component: DepartmentsubsectioneditComponent;
  let fixture: ComponentFixture<DepartmentsubsectioneditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DepartmentsubsectioneditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DepartmentsubsectioneditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
