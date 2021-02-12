import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBasketComponent } from './side-basket.component';

describe('SideBasketComponent', () => {
  let component: SideBasketComponent;
  let fixture: ComponentFixture<SideBasketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SideBasketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SideBasketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
