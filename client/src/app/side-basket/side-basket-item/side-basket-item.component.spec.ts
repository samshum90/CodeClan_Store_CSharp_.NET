import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBasketItemComponent } from './side-basket-item.component';

describe('SideBasketItemComponent', () => {
  let component: SideBasketItemComponent;
  let fixture: ComponentFixture<SideBasketItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SideBasketItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SideBasketItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
