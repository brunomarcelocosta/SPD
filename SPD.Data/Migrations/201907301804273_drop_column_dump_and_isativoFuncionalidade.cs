namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drop_column_dump_and_isativoFuncionalidade : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SPD_HISTORICO_OPERACAO", "de_dump");
            DropColumn("dbo.SPD_FUNCIONALIDADE", "lg_ativo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SPD_FUNCIONALIDADE", "lg_ativo", c => c.Boolean(nullable: false));
            AddColumn("dbo.SPD_HISTORICO_OPERACAO", "de_dump", c => c.String(maxLength: 4000));
        }
    }
}
