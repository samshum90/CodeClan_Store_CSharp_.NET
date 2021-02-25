import { TestBed } from '@angular/core/testing';

import { BasketResolver } from './basket.resolver';

describe('BasketResolver', () => {
  let resolver: BasketResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(BasketResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
