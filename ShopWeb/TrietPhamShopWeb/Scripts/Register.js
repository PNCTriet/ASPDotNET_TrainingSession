console.log('Script loaded'); // Kiểm tra script đã được load

document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM Content Loaded - Starting password strength meter initialization');

    // Get elements using ASP.NET TextBox IDs
    const passwordInput = document.querySelector('#MainContent_txtPassword');
    const confirmPasswordInput = document.querySelector('#MainContent_txtConfirmPassword');
    const strengthBar = document.querySelector('.progress-bar');
    const strengthText = document.querySelector('#password-strength-text');
    const registerButton = document.querySelector('#MainContent_btnRegister');

    // Debug logs for all elements
    console.log('Elements found:');
    console.log('Password Input:', passwordInput);
    console.log('Confirm Password Input:', confirmPasswordInput);
    console.log('Strength Bar:', strengthBar);
    console.log('Strength Text:', strengthText);
    console.log('Register Button:', registerButton);

    if (passwordInput && strengthBar && strengthText) {
        console.log('All required elements found, setting up event listeners');
        
        passwordInput.addEventListener('input', function() {
            console.log('Password input changed');
            const password = this.value;
            console.log('Current password length:', password.length);
            
            const result = zxcvbn(password);
            console.log('Password strength result:', result);
            
            // Update progress bar
            const strength = result.score;
            const width = (strength + 1) * 20; // Convert 0-4 to 25-100%
            console.log('Setting strength bar width to:', width + '%');
            strengthBar.style.width = width + '%';

            // Update colors and text based on strength
            let strengthClass = '';
            let strengthMessage = '';
            
            switch(strength) {
                case 0:
                    strengthClass = 'bg-danger';
                    strengthMessage = 'Rất yếu';
                    break;
                case 1:
                    strengthClass = 'bg-warning';
                    strengthMessage = 'Trung Bình';
                    break;
                case 2:
                    strengthClass = 'bg-info';
                    strengthMessage = 'Khá';
                    break;
                case 3:
                    strengthClass = 'bg-primary';
                    strengthMessage = 'Mạnh';
                    break;
                case 4:
                    strengthClass = 'bg-success';
                    strengthMessage = 'Rất mạnh';
                    break;
            }
            
            console.log('Updating strength class to:', strengthClass);
            strengthBar.className = 'progress-bar ' + strengthClass;
            strengthText.textContent = strengthMessage;

            // Show feedback if available
            if (result.feedback.warning) {
                strengthText.textContent += ' - ' + result.feedback.warning;
                console.log('Added warning feedback:', result.feedback.warning);
            }

            // Disable register button if password is too weak
            if (registerButton) {
                const isDisabled = strength < 2;
                console.log('Setting register button disabled:', isDisabled);
                registerButton.disabled = isDisabled;
                if (isDisabled) {
                    registerButton.title = 'Mật khẩu phải có độ mạnh từ Khá trở lên';
                } else {
                    registerButton.title = '';
                }
            }
        });

        // Check password match
        if (confirmPasswordInput) {
            confirmPasswordInput.addEventListener('input', function() {
                const password = passwordInput.value;
                const confirmPassword = this.value;
                console.log('Confirm password changed, checking match');

                if (password !== confirmPassword) {
                    this.setCustomValidity('Mật khẩu xác nhận không khớp');
                    console.log('Passwords do not match');
                } else {
                    this.setCustomValidity('');
                    console.log('Passwords match');
                }
            });
        }
    } else {
        console.error('Missing required elements for password strength meter:');
        console.error('Password Input found:', !!passwordInput);
        console.error('Strength Bar found:', !!strengthBar);
        console.error('Strength Text found:', !!strengthText);
        console.error('Register Button found:', !!registerButton);
    }
});

