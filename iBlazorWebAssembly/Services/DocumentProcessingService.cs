using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Presentation;
using iBlazorWebAssembly.Models;
using Markdig;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace iBlazorWebAssembly.Services
{
    /// <summary>
    /// 文档处理服务实现，用于处理不同类型的文档
    /// </summary>
    public class DocumentProcessingService : IDocumentProcessingService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string[] _supportedFileTypes = {
            ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt", ".pdf", ".md", ".txt"
        };

        public DocumentProcessingService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// 从文档中提取文本
        /// </summary>
        public async Task<string> ExtractTextFromDocumentAsync(byte[] content, string fileType)
        {
            fileType = fileType.ToLowerInvariant();

            try
            {
                // 对于可能耗时的操作，使用 Task.Run 包装
                return await Task.Run(() => {
                    return fileType switch
                    {
                        ".docx" => ExtractTextFromDocx(content),
                        ".xlsx" => ExtractTextFromXlsx(content),
                        ".pptx" => ExtractTextFromPptx(content),
                        ".md" => ExtractTextFromMarkdown(content),
                        ".txt" => ExtractTextFromTxt(content),
                        _ => "不支持的文件类型"
                    };
                });
            }
            catch (Exception ex)
            {
                return $"提取文本时出错: {ex.Message}";
            }
        }

        /// <summary>
        /// 将文档转换为HTML
        /// </summary>
        public async Task<string> ConvertDocumentToHtmlAsync(byte[] content, string fileType)
        {
            fileType = fileType.ToLowerInvariant();

            try
            {
                return fileType switch
                {
                    ".md" => ConvertMarkdownToHtml(content),
                    ".docx" => await ConvertDocxToHtmlAsync(content),
                    _ => $"<pre>{await ExtractTextFromDocumentAsync(content, fileType)}</pre>"
                };
            }
            catch (Exception ex)
            {
                return $"<p>转换HTML时出错: {ex.Message}</p>";
            }
        }

        /// <summary>
        /// 从IBrowserFile中处理文档并创建元数据
        /// </summary>
        public async Task<DocumentMetadata> ProcessDocumentFromBrowserFileAsync(IBrowserFile file, bool extractText = true)
        {
            // 验证文件类型
            if (!IsFileTypeSupported(file.Name))
            {
                throw new InvalidOperationException($"不支持的文件类型: {Path.GetExtension(file.Name)}");
            }

            // 读取文件内容
            using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 最大10MB
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] fileContent = memoryStream.ToArray();

            // 创建文档元数据
            var metadata = new DocumentMetadata
            {
                FileName = file.Name,
                FileType = Path.GetExtension(file.Name).ToLowerInvariant(),
                FileSize = file.Size,
                ContentBase64 = Convert.ToBase64String(fileContent),
                Title = Path.GetFileNameWithoutExtension(file.Name)
            };

            // 提取文本（如果需要）
            if (extractText)
            {
                var text = await ExtractTextFromDocumentAsync(fileContent, metadata.FileType);
                if (text.Length > 500) // 限制摘要长度
                {
                    metadata.Description = text.Substring(0, 500) + "...";
                }
                else
                {
                    metadata.Description = text;
                }
            }

            return metadata;
        }

        /// <summary>
        /// 验证文件类型是否支持
        /// </summary>
        public bool IsFileTypeSupported(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return _supportedFileTypes.Contains(extension);
        }

        /// <summary>
        /// 获取支持的文件类型列表
        /// </summary>
        public string[] GetSupportedFileTypes()
        {
            return _supportedFileTypes;
        }

        /// <summary>
        /// 创建文档的新版本
        /// </summary>
        public async Task<DocumentMetadata> CreateDocumentVersionAsync(DocumentMetadata originalDocument, IBrowserFile file, string versionNote)
        {
            // 验证文件类型与原始文档一致
            var fileExtension = Path.GetExtension(file.Name).ToLowerInvariant();
            if (fileExtension != originalDocument.FileType)
            {
                throw new InvalidOperationException($"新版本文件类型 ({fileExtension}) 与原始文档类型 ({originalDocument.FileType}) 不匹配");
            }

            // 处理新版本文件
            var newVersion = await ProcessDocumentFromBrowserFileAsync(file, true);
            
            // 增加版本号
            var versionParts = originalDocument.Version.Split('.');
            if (versionParts.Length == 2 && int.TryParse(versionParts[1], out int minorVersion))
            {
                newVersion.Version = $"{versionParts[0]}.{minorVersion + 1}";
            }
            else
            {
                // 如果版本号格式不正确，设置为下一个版本
                newVersion.Version = "1.0";
            }
            
            // 设置版本信息
            newVersion.Title = originalDocument.Title; // 保持标题一致
            newVersion.VersionNote = versionNote;
            newVersion.IsHistoryVersion = false;
            newVersion.ParentDocumentId = null;
            
            // 复制原始文档的一些属性
            newVersion.Tags = originalDocument.Tags;
            newVersion.Category = originalDocument.Category;
            newVersion.Author = originalDocument.Author;
            newVersion.IsPublic = originalDocument.IsPublic;
            
            // 处理原始文档，设置为历史版本
            originalDocument.IsHistoryVersion = true;
            originalDocument.ParentDocumentId = newVersion.Id;
            
            // 合并版本历史
            newVersion.VersionHistory = new List<string>();
            newVersion.VersionHistory.Add(originalDocument.Id);
            if (originalDocument.VersionHistory != null && originalDocument.VersionHistory.Any())
            {
                newVersion.VersionHistory.AddRange(originalDocument.VersionHistory);
            }
            
            return newVersion;
        }

        /// <summary>
        /// 获取文档的所有版本
        /// </summary>
        public async Task<List<DocumentMetadata>> GetDocumentVersionsAsync(string documentId)
        {
            // 注意：此方法需要与存储服务（如LocalStorageMetadataService）结合使用
            // 在这里仅定义该方法的逻辑架构，实际使用需要依赖存储服务来获取文档
            
            // 因为这是一个异步方法，但目前没有实际的异步操作，使用 Task.FromException 返回异常
            return await Task.FromException<List<DocumentMetadata>>(
                new NotImplementedException("此方法需要与存储服务结合使用，在实际应用中实现")
            );
        }

        /// <summary>
        /// 比较两个文档版本的差异
        /// </summary>
        public async Task<string> CompareDocumentVersionsAsync(string originalDocumentId, string newDocumentId)
        {
            // 注意：此方法需要与存储服务结合使用，在此仅定义逻辑架构
            // 在实际应用中，需要从存储中获取两个文档，然后进行比较
            
            // 因为这是一个异步方法，但目前没有实际的异步操作，使用 Task.FromException 返回异常
            return await Task.FromException<string>(
                new NotImplementedException("需要实现文档比较功能")
            );
        }
        
        /// <summary>
        /// 恢复到指定的文档版本
        /// </summary>
        public async Task<DocumentMetadata> RestoreDocumentVersionAsync(DocumentMetadata currentDocument, DocumentMetadata versionToRestore)
        {
            // 验证参数
            if (currentDocument == null || versionToRestore == null)
            {
                throw new ArgumentNullException("当前文档和要恢复的版本不能为空");
            }
            
            // 验证版本关系 - 确保要恢复的版本确实是当前文档的历史版本
            bool isValidVersion = versionToRestore.ParentDocumentId == currentDocument.Id || 
                                 (currentDocument.VersionHistory != null && 
                                  currentDocument.VersionHistory.Contains(versionToRestore.Id));
            
            if (!isValidVersion)
            {
                throw new InvalidOperationException("无法恢复不相关的文档版本");
            }
            
            // 创建恢复后的新版本（使用Task.Run以确保异步执行）
            return await Task.Run(() => {
                var restoredVersion = new DocumentMetadata
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = currentDocument.Title,
                    FileName = versionToRestore.FileName,
                    FileType = versionToRestore.FileType,
                    FileSize = versionToRestore.FileSize,
                    ContentBase64 = versionToRestore.ContentBase64,
                    Description = versionToRestore.Description,
                    Author = currentDocument.Author,
                    Category = currentDocument.Category,
                    Tags = currentDocument.Tags,
                    IsPublic = currentDocument.IsPublic,
                    CreatedAt = DateTime.Now, // 使用 CreatedAt 而不是 CreatedDate
                    ModifiedDate = DateTime.Now,
                    VersionNote = $"从版本 {versionToRestore.Version} 恢复",
                    Version = IncrementVersion(currentDocument.Version),
                    IsHistoryVersion = false,
                    ParentDocumentId = null
                };
                
                // 设置版本历史
                restoredVersion.VersionHistory = new List<string> { currentDocument.Id };
                if (currentDocument.VersionHistory != null && currentDocument.VersionHistory.Any())
                {
                    restoredVersion.VersionHistory.AddRange(currentDocument.VersionHistory);
                }
                
                // 设置当前版本为历史版本
                currentDocument.IsHistoryVersion = true;
                currentDocument.ParentDocumentId = restoredVersion.Id;
                
                return restoredVersion;
            });
        }

        /// <summary>
        /// 还原到指定版本
        /// </summary>
        /// <param name="versionDocumentId">要还原的版本ID</param>
        /// <returns>还原后的文档元数据</returns>
        public async Task<DocumentMetadata> RestoreDocumentVersionAsync(string versionDocumentId)
        {
            // 注意：此方法需要与存储服务结合使用，在此仅定义逻辑架构
            // 在实际应用中，需要：
            // 1. 通过ID获取要还原的版本文档
            // 2. 通过ParentDocumentId或VersionHistory找到当前版本文档
            // 3. 调用RestoreDocumentVersionAsync(currentDocument, versionToRestore)完成还原
            
            // 因为这是一个异步方法，但目前没有实际的异步操作，使用 Task.FromException 返回异常
            return await Task.FromException<DocumentMetadata>(
                new NotImplementedException("需要实现文档版本还原功能，请在调用代码中结合IMetadataService使用")
            );
        }

        #region 私有文档处理方法

        /// <summary>
        /// 从Word文档提取文本
        /// </summary>
        private string ExtractTextFromDocx(byte[] content)
        {
            using var memoryStream = new MemoryStream(content);
            using var wordDocument = WordprocessingDocument.Open(memoryStream, false);
            
            var body = wordDocument.MainDocumentPart?.Document.Body;
            if (body != null)
            {
                return body.InnerText;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// 从Excel文档提取文本
        /// </summary>
        private string ExtractTextFromXlsx(byte[] content)
        {
            using var memoryStream = new MemoryStream(content);
            using var spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, false);
            
            var text = new System.Text.StringBuilder();
            var workbookPart = spreadsheetDocument.WorkbookPart;
            
            if (workbookPart != null)
            {
                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                foreach (var sheet in sheets)
                {
                    if (sheet.Id?.Value != null)
                    {
                        var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id.Value);
                        var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                        
                        foreach (var row in sheetData.Elements<Row>())
                        {
                            foreach (var cell in row.Elements<Cell>())
                            {
                                if (cell.CellValue != null)
                                {
                                    text.Append(cell.CellValue.Text);
                                    text.Append(" ");
                                }
                            }
                            text.AppendLine();
                        }
                    }
                }
            }
            
            return text.ToString();
        }

        /// <summary>
        /// 从PowerPoint文档提取文本
        /// </summary>
        private string ExtractTextFromPptx(byte[] content)
        {
            using var memoryStream = new MemoryStream(content);
            using var presentationDocument = PresentationDocument.Open(memoryStream, false);
            
            var text = new System.Text.StringBuilder();
            var presentationPart = presentationDocument.PresentationPart;
            
            if (presentationPart?.Presentation?.SlideIdList != null)
            {
                var slideIds = presentationPart.Presentation.SlideIdList.ChildElements;
                foreach (var slideId in slideIds)
                {
                    if (slideId is SlideId sid)
                    {
                        // 使用非空断言运算符，明确告诉编译器 RelationshipId 不为 null
                        string? slidePartId = sid.RelationshipId;
                        
                        // 在这里添加空检查，确保 slidePartId 不为 null
                        if (!string.IsNullOrEmpty(slidePartId))
                        {
                            try
                            {
                                // 直接使用变量而不是属性，避免空引用警告
                                OpenXmlPart? part = presentationPart.GetPartById(slidePartId);
                                if (part is SlidePart slidePart && slidePart.Slide != null)
                                {
                                    // 提取幻灯片中的文本
                                    var paragraphs = slidePart.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Paragraph>();
                                    foreach (var paragraph in paragraphs)
                                    {
                                        text.AppendLine(paragraph.InnerText);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                text.AppendLine($"读取幻灯片错误: {ex.Message}");
                            }
                        }
                    }
                }
            }
            
            return text.ToString();
        }

        /// <summary>
        /// 从Markdown文件提取文本
        /// </summary>
        private string ExtractTextFromMarkdown(byte[] content)
        {
            var markdownText = System.Text.Encoding.UTF8.GetString(content);
            return markdownText; // 直接返回Markdown文本
        }

        /// <summary>
        /// 从文本文件提取文本
        /// </summary>
        private string ExtractTextFromTxt(byte[] content)
        {
            return System.Text.Encoding.UTF8.GetString(content);
        }

        /// <summary>
        /// 将Markdown转换为HTML
        /// </summary>
        private string ConvertMarkdownToHtml(byte[] content)
        {
            var markdownText = System.Text.Encoding.UTF8.GetString(content);
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdig.Markdown.ToHtml(markdownText, pipeline);
        }

        /// <summary>
        /// 将Word文档转换为HTML
        /// </summary>
        private async Task<string> ConvertDocxToHtmlAsync(byte[] content)
        {
            // 简化实现：提取文本并用HTML段落包装，使用Task.Run确保异步执行
            return await Task.Run(() => {
                var text = ExtractTextFromDocx(content);
                var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                var html = new System.Text.StringBuilder();
                html.Append("<div class=\"document-content\">");
                
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        html.Append($"<p>{System.Net.WebUtility.HtmlEncode(line)}</p>");
                    }
                }
                
                html.Append("</div>");
                return html.ToString();
            });
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 生成版本差异的HTML
        /// </summary>
        private string GenerateDiffHtml(string[] originalLines, string[] newLines)
        {
            var diffHtml = new System.Text.StringBuilder();
            diffHtml.Append("<div class=\"diff-container\">");
            
            int maxLines = Math.Max(originalLines.Length, newLines.Length);
            
            for (int i = 0; i < maxLines; i++)
            {
                string originalLine = i < originalLines.Length ? originalLines[i] : "";
                string newLine = i < newLines.Length ? newLines[i] : "";
                
                if (originalLine != newLine)
                {
                    diffHtml.Append("<div class=\"diff-line\">");
                    diffHtml.Append($"<div class=\"diff-old\">{System.Net.WebUtility.HtmlEncode(originalLine)}</div>");
                    diffHtml.Append($"<div class=\"diff-new\">{System.Net.WebUtility.HtmlEncode(newLine)}</div>");
                    diffHtml.Append("</div>");
                }
                else
                {
                    diffHtml.Append($"<div class=\"diff-unchanged\">{System.Net.WebUtility.HtmlEncode(originalLine)}</div>");
                }
            }
            
            diffHtml.Append("</div>");
            return diffHtml.ToString();
        }

        /// <summary>
        /// 递增版本号
        /// </summary>
        private string IncrementVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return "1.0";
            }
            
            var versionParts = version.Split('.');
            if (versionParts.Length == 2 && int.TryParse(versionParts[1], out int minorVersion))
            {
                return $"{versionParts[0]}.{minorVersion + 1}";
            }
            else
            {
                return "1.0";
            }
        }

        #endregion
    }
}