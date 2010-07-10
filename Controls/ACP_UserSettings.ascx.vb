'
' DotNetNukeŽ - http://www.dotnetnuke.com
' Copyright (c) 2002-2010
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Option Strict On
Option Explicit On 

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This is the General Settings area from Forum Administration.
	''' Set basic configuration items for this forum module. (mod settings)
	''' </summary>
	''' <remarks>The majority of how the module operates is set here.
	''' </remarks>
	Partial Public Class UserSettings
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This interface is used to replace a If Page.IsPostBack typically used in page load. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			BindMemberNameTypes()
			ddlNameDisplay.SelectedIndex = objConfig.ForumMemberName
			chkTrustNewUsers.Checked = objConfig.TrustNewUsers
			chkAutoLockTrust.Checked = objConfig.AutoLockTrust
			chkEnableUserSignatures.Checked = objConfig.EnableUserSignatures
			chkEnableModSigUpdates.Checked = objConfig.EnableModSigUpdates
			chkEnableHTMLSignatures.Checked = objConfig.EnableHTMLSignatures
			txtPostEditWindow.Text = CStr(objConfig.PostEditWindow)
			chkEnableUserBanning.Checked = objConfig.EnableUserBanning
			chkUserReadManagement.Checked = objConfig.EnableUserReadManagement

			'CP - COMEBACK: Handle when we are able to impersonate security of other users.
			chkHideModEdit.Checked = False 'objConfig.HideModEdits
			SetVisibleItems()
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Updates the module's configuration (module settings)
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Saves the module settings shown in this view.
		''' </remarks>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				Dim ctlModule As New Entities.Modules.ModuleController
				ctlModule.UpdateModuleSetting(ModuleId, Constants.MEMBER_NAME_DISPLAY_FORMAT, ddlNameDisplay.SelectedValue)

				If txtPostEditWindow.Text <> String.Empty Then
					ctlModule.UpdateModuleSetting(ModuleId, Constants.POST_EDIT_WINDOW, txtPostEditWindow.Text)
				Else
					ctlModule.UpdateModuleSetting(ModuleId, Constants.POST_EDIT_WINDOW, "0")
				End If

				ctlModule.UpdateModuleSetting(ModuleId, Constants.TRUST_NEW_USERS, chkTrustNewUsers.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.AUTO_LOCK_TRUST, chkAutoLockTrust.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_USER_READ_MANAGEMENT, chkUserReadManagement.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_USER_SIGNATURES, chkEnableUserSignatures.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_MOD_SIGNATURE_EDITS, chkEnableModSigUpdates.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_HTML_IN_SIGNATURES, chkEnableHTMLSignatures.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.HIDE_MODERATOR_EDITS, chkHideModEdit.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_USER_BANNING, chkEnableUserBanning.Checked.ToString)

				Configuration.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Enables/Disables user signature related rows depending on selection.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Changes viewable/editable options when checked/unchecked.</remarks>
		Protected Sub chkEnableUserSignatures_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableUserSignatures.CheckedChanged
			SetVisibleItems()
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Sets visiblity for rows on page depending on user signatures being enabled.
		''' </summary>
		''' <remarks>Uses forum configuration settings to show/hide items.</remarks>
		Private Sub SetVisibleItems()
			If chkEnableUserSignatures.Checked Then
				rowModSigUpdates.Visible = True
				rowHTMLSignatures.Visible = True
			Else
				rowModSigUpdates.Visible = False
				rowHTMLSignatures.Visible = False
			End If
		End Sub

		''' <summary>
		''' Binds the various member name display options, pulled from core Lists table. 
		''' </summary>
		''' <remarks>Uses lists localized items to determine options.</remarks>
		Private Sub BindMemberNameTypes()
			ddlNameDisplay.ClearSelection()

			ddlNameDisplay.Items.Insert(0, New ListItem(Localization.GetString("Username", objConfig.SharedResourceFile), "0"))
			ddlNameDisplay.Items.Insert(1, New ListItem(Localization.GetString("DisplayName", objConfig.SharedResourceFile), "1"))

			' Now Bind the items
			ddlNameDisplay.Items.FindByValue(objConfig.ForumMemberName.ToString).Selected = True
		End Sub

#End Region

	End Class

End Namespace