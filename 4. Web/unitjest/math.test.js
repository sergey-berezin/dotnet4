const m = require('./math.js');

test('adds 1 + 2 to equal 3', () => {
  expect(m.sum(1, 2)).toBe(3);
});

test('multiplies 1 * 2 to equal 2', () => {
  expect(m.prod(1, 2)).toBe(2);
});