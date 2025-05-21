// Power BI Embed Configuration
let powerbi = window.powerbi;
let embedConfig = null;
let dashboard = null;

// Initialize the application when the DOM is fully loaded
document.addEventListener('DOMContentLoaded', function () {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Set up event listeners for the dashboard modal
    const dashboardModal = document.getElementById('dashboardModal');
    if (dashboardModal) {
        dashboardModal.addEventListener('shown.bs.modal', function () {
            initializeDashboard();
        });

        dashboardModal.addEventListener('hidden.bs.modal', function () {
            // Clean up when modal is closed
            if (dashboard) {
                dashboard.off('loaded');
                dashboard.off('error');
                dashboard = null;
            }
        });
    }
});

// Initialize the Power BI dashboard
async function initializeDashboard() {
    try {
        showLoading(true);
        
        // Get the embed configuration from the server
        const response = await fetch('/?handler=EmbedInfo');
        if (!response.ok) {
            throw new Error(`Failed to load dashboard: ${response.status} ${response.statusText}`);
        }
        
        const data = await response.json();
        
        // Configure the embed configuration
        embedConfig = {
            type: 'dashboard',
            tokenType: 1, // AAD Token
            accessToken: data.token,
            embedUrl: data.embedUrl,
            settings: {
                panes: {
                    filters: {
                        expanded: false,
                        visible: true
                    }
                },
                background: 1, // Use theme background
                filterPaneEnabled: true,
                navContentPaneEnabled: true
            }
        };
        
        // Get the container element
        const embedContainer = document.getElementById('embedContainer');
        if (!embedContainer) {
            throw new Error('Could not find the embed container');
        }
        
        // Clear any existing content
        embedContainer.innerHTML = '';
        
        // Embed the dashboard
        dashboard = powerbi.embed(embedContainer, embedConfig);
        
        // Handle dashboard events
        dashboard.on('loaded', function() {
            console.log('Dashboard loaded successfully');
            showLoading(false);
        });
        
        dashboard.on('error', function(event) {
            console.error('Error loading dashboard:', event.detail);
            showError('Failed to load dashboard. Please try again.');
            showLoading(false);
        });
        
    } catch (error) {
        console.error('Error initializing dashboard:', error);
        showError(error.message || 'An error occurred while loading the dashboard.');
        showLoading(false);
    }
}

// Show loading indicator
function showLoading(show) {
    const loadingElement = document.getElementById('loadingIndicator');
    if (loadingElement) {
        loadingElement.style.display = show ? 'flex' : 'none';
    }
}

// Show error message
function showError(message) {
    const errorElement = document.getElementById('errorMessage');
    if (errorElement) {
        errorElement.textContent = message;
        errorElement.style.display = 'block';
    }
}

// Resize handler for responsive embedding
function resizeDashboard() {
    if (dashboard) {
        try {
            dashboard.off('loaded');
            dashboard.off('error');
            dashboard = null;
            initializeDashboard();
        } catch (error) {
            console.error('Error resizing dashboard:', error);
        }
    }
}

// Add window resize event with debounce
let resizeTimer;
window.addEventListener('resize', function() {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(resizeDashboard, 250);
});
