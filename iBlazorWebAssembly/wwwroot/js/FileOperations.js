// FileOperations.js - 处理文件相关的JavaScript操作

/**
 * 文件操作工具类
 * 提供文件选择、下载等功能
 */

window.fileOperations = {
    /**
     * 触发文件选择输入框
     * @param {string} inputId - 文件输入元素的ID
     */
    openFileInput: function (inputId) {
        const fileInput = document.getElementById(inputId);
        if (fileInput) {
            fileInput.click();
        } else {
            console.error(`文件输入元素没有找到: ${inputId}`);
        }
    },

    /**
     * 在新标签页中打开URL
     * @param {string} url - 要打开的URL
     */
    openInNewTab: function (url) {
        window.open(url, '_blank');
    },

    /**
     * 下载指定URL的文件
     * @param {string} url - 文件URL
     * @param {string} fileName - 文件名
     */
    downloadFile: function (url, fileName) {
        fetch(url)
            .then(response => response.blob())
            .then(blob => {
                const blobUrl = URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.style.display = 'none';
                a.href = blobUrl;
                a.download = fileName;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
                URL.revokeObjectURL(blobUrl);
            })
            .catch(error => console.error('下载文件出错:', error));
    },

    /**
     * 将文本复制到剪贴板
     * @param {string} text - 要复制的文本
     * @returns {Promise<boolean>} - 是否成功复制
     */
    copyToClipboard: async function (text) {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (error) {
            console.error('复制到剪贴板失败:', error);
            return false;
        }
    },

    /**
     * 从base64字符串下载文件
     * @param {string} base64Data - base64编码的文件数据
     * @param {string} fileName - 文件名
     * @param {string} mimeType - 文件MIME类型
     */
    downloadBase64File: function (base64Data, fileName, mimeType) {
        const byteCharacters = atob(base64Data);
        const byteArrays = [];

        for (let offset = 0; offset < byteCharacters.length; offset += 1024) {
            const slice = byteCharacters.slice(offset, offset + 1024);
            const byteNumbers = new Array(slice.length);

            for (let i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            const byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }

        const blob = new Blob(byteArrays, { type: mimeType });
        const blobUrl = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = blobUrl;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(blobUrl);
    },

    /**
     * 读取文件为ArrayBuffer，并转换为Blazor兼容的格式
     * @param {File} file - 要读取的文件对象
     * @returns {Promise<Uint8Array>} - 返回文件的字节数组，兼容Blazor JS Interop
     */
    readFileAsArrayBuffer: async function (file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => resolve(new Uint8Array(reader.result));
            reader.onerror = () => reject(reader.error);
            reader.readAsArrayBuffer(file);
        });
    },

    /**
     * 获取文件对象引用
     * @param {string} inputId - 文件输入元素的ID
     * @param {number} index - 文件索引，默认为0
     * @returns {File|null} - 返回文件对象或null
     */
    getFileFromInput: function (inputId, index = 0) {
        const input = document.getElementById(inputId);
        if (input && input.files && input.files.length > index) {
            return input.files[index];
        }
        return null;
    },

    /**
     * 获取文件对象的属性
     * @param {File} file - 文件对象
     * @returns {Object|null} - 返回文件属性对象或null
     */
    getFileProperties: function (file) {
        if (!file) return null;
        
        return {
            name: file.name,
            size: file.size,
            type: file.type,
            lastModified: file.lastModified
        };
    }
};