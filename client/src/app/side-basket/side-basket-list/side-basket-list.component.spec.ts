import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBasketListComponent } from './side-basket-list.component';

describe('SideBasketListComponent', () => {
  let component: SideBasketListComponent;
  let fixture: ComponentFixture<SideBasketListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SideBasketListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SideBasketListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
