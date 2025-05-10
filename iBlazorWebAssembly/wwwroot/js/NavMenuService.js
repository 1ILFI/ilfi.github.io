// 拖动状态相关变量
let isDragging = false;
let offsetX = 0;
let offsetY = 0;
let navElement = null;

// 启动拖动
window.startDragging = function (elementId) {
    navElement = document.getElementById(elementId);
    if (!navElement) return;

    // 获取当前事件（从window.event，确保与浏览器兼容性）
    const event = window.event;
    if (!event) return;

    isDragging = true;

    // 获取鼠标在元素内的相对位置
    const rect = navElement.getBoundingClientRect();
    offsetX = event.clientX - rect.left;
    offsetY = event.clientY - rect.top;

    // 重要：重置元素的transform，并使用顶层的位置样式
    navElement.style.transition = 'none'; // 禁用过渡效果
    navElement.style.top = rect.top + 'px';
    navElement.style.left = rect.left + 'px';
    navElement.style.transform = 'none';

    // 添加鼠标移动事件处理 - 使用 pointermove 提高响应速度
    document.addEventListener('pointermove', handlePointerMove, { passive: false });
    document.addEventListener('pointerup', stopDragging);

    // 添加拖动时的视觉样式
    navElement.style.cursor = 'grabbing';
    document.body.style.userSelect = 'none';

    // 阻止默认行为可能导致的额外延迟
    if (event.preventDefault) event.preventDefault();
    return false;
};

// 停止拖动
function stopDragging() {
    if (!isDragging || !navElement) return;

    isDragging = false;
    document.removeEventListener('pointermove', handlePointerMove, { passive: false });
    document.removeEventListener('pointerup', stopDragging);

    // 恢复样式
    navElement.style.cursor = '';
    navElement.style.transition = '';
    document.body.style.userSelect = '';

    // 更新最终位置数据属性
    const rect = navElement.getBoundingClientRect();
    navElement.dataset.posX = rect.left;
    navElement.dataset.posY = rect.top;
}

// 导出stopDragging到window对象，使其可从外部访问
window.stopDragging = stopDragging;

// 优化的指针移动处理
function handlePointerMove(e) {
    if (!isDragging || !navElement) return;

    // 阻止默认行为和事件冒泡以提高性能
    e.preventDefault();
    e.stopPropagation();

    // 直接更新元素位置（不使用transform）
    const newLeft = e.clientX - offsetX;
    const newTop = e.clientY - offsetY;

    // 防止拖出视口范围
    const maxLeft = window.innerWidth - navElement.offsetWidth;
    const maxTop = window.innerHeight - navElement.offsetHeight;

    // 应用位置限制
    navElement.style.left = Math.max(0, Math.min(maxLeft, newLeft)) + 'px';
    navElement.style.top = Math.max(0, Math.min(maxTop, newTop)) + 'px';
}

// 获取导航位置
window.getNavPosition = function (elementId) {
    const element = document.getElementById(elementId);
    if (!element) return null;

    return {
        top: element.style.top || '100px',
        left: element.style.left || '20px'
    };
};

// 初始化导航菜单
window.initNavMenu = function() {
    // 全局事件处理，确保在任何情况下都能正确停止拖动
    document.addEventListener('pointercancel', stopDragging);
    window.addEventListener('blur', stopDragging);

    // 初始化导航条位置 - 从localStorage恢复
    try {
        const savedPosition = localStorage.getItem('navMenuPosition');
        if (savedPosition) {
            const positions = JSON.parse(savedPosition);
            const navMenu = document.getElementById('floatingNavMenu');
            if (navMenu && positions && positions.top && positions.left) {
                // 直接应用保存的位置，不使用transform
                navMenu.style.top = positions.top;
                navMenu.style.left = positions.left;
                navMenu.style.transform = 'none'; // 清除可能的transform
            }
        }
    } catch (error) {
        console.error('Error initializing nav position:', error);
    }
};