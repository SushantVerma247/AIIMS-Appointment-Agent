using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentAgent.Worker.Services
{
    public class PlaywrightService : IAsyncDisposable
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _context;
        private IPage? _page;

        public async Task InitializeAsync(bool headless = false)
        {
            if (_browser != null)
                return;

            _playwright = await Playwright.CreateAsync();

            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = headless,
                SlowMo = 100
            });

            _context = await _browser.NewContextAsync();

            _page = await _context.NewPageAsync();
        }

        public async Task NavigateAsync(string url)
        {
            await InitializeAsync();

            await _page!.GotoAsync(url);
        }

        public async Task ClickAsync(string selector)
        {
            var locator = _page!.Locator(selector);

            await locator.WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible
            });

            await locator.ScrollIntoViewIfNeededAsync();

            await locator.ClickAsync(new()
            {
                Timeout = 10000
            });
        }

        public async Task FillAsync(string selector, string value)
        {
            await _page!.Locator(selector).FillAsync(value);
        }

        public async Task SelectOptionAsync(string selector, string value)
        {
            await _page!.Locator(selector).SelectOptionAsync(value);
        }

        public async Task<string> GetTextAsync(string selector)
        {
            return await _page!.Locator(selector).InnerTextAsync();
        }

        public async Task<bool> ExistsAsync(string selector)
        {
            return await _page!.Locator(selector).IsVisibleAsync();
        }

        public async Task WaitAsync(string selector)
        {
            await _page!.Locator(selector).WaitForAsync();
        }

        public async Task ScreenshotAsync(string fileName)
        {
            await _page!.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = fileName,
                FullPage = true
            });
        }

        public async Task<IReadOnlyList<ILocator>> GetElementsAsync(string selector)
        {
            return await _page!.Locator(selector).AllAsync();
        }

        public IPage Page => _page!;

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
                await _context.CloseAsync();

            if (_browser != null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
        }

        public async Task<string?> GetAttributeAsync(ILocator locator, string attribute)
        {
            return await locator.GetAttributeAsync(attribute);
        }

        public async Task WaitForVisibleAsync(string selector)
        {
            await _page!
                .Locator(selector)
                .WaitForAsync(new()
                {
                    State = WaitForSelectorState.Visible
                });
        }

        public async Task WaitForNetworkIdleAsync()
        {
            await _page!.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task WaitUntilTextChangesAsync(string selector, string oldText, int timeout = 10000)
        {
            var locator = _page!.Locator(selector);

            await locator.WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeout
            });

            await _page.WaitForFunctionAsync(
                @"([selector, text]) => {
                const el = document.querySelector(selector);
                return el && el.textContent.trim() !== text;}",
                new object[] { selector, oldText },
                new PageWaitForFunctionOptions
                {
                    Timeout = timeout
                });
        }

        public async Task CloseBrowserAsync()
        {
            if (_context != null)
            {
                await _context.CloseAsync();
                _context = null;
            }

            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }

            _playwright?.Dispose();
            _playwright = null;

            _page = null;
        }
    }
}
