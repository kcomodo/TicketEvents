import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotUserComponent } from './forgot-user.component';

describe('ForgotUserComponent', () => {
  let component: ForgotUserComponent;
  let fixture: ComponentFixture<ForgotUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ForgotUserComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ForgotUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
